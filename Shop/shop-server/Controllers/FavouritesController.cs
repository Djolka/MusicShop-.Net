using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using System.Linq.Expressions;
using System.CodeDom;
using System.Diagnostics.Eventing.Reader;
using System;

namespace MusicShop.Controllers
{
    [ApiController]
    [Route("favourites")]
    public class FavouritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavouritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllFavourites")]
        public async Task<ActionResult<List<Favourite>>> GetAllFavourites()
        {
            var favs = await _context.Favourites.ToListAsync();

            if (favs == null)
            {
                return NotFound();
            }

            return Ok(favs);
        }

        [HttpGet("getFavourites/{id}")]
        public async Task<ActionResult<List<Favourite>>> GetFavouritesByUser(string id) {
            var favsByUser = await _context.Favourites
                                    .Include(f => f.Product)
                                    .Where(f => f.CustomerId == id)
                                    .ToListAsync();

            return Ok(favsByUser);
        }

        [HttpPost("addFavourites")]
        public async Task<ActionResult<Favourite>> AddFavourite([FromBody] Favourite fav)
        {
            try
            {
                _context.Attach(fav.Product);
                _context.Favourites.Add(fav);
                await _context.SaveChangesAsync();

                return Ok(fav);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = "An error occurred while saving the favourite.",
                    message = e.Message,
                    inner = e.InnerException?.Message,
                    stackTrace = e.StackTrace
                });
            }
        }

        [HttpPost("findFavourite")]
        public async Task<ActionResult<object>> FindFavourite([FromBody] Favourite fav)
        {
            var found = await _context.Favourites.AnyAsync(f => f.CustomerId.Equals(fav.CustomerId) && f.Product.Id.Equals(fav.Product.Id));

            if (!found)
            {
                return Ok(new { found = false });
            }

            return Ok(new { found = true });
        }

        [HttpDelete("deleteFavourite/{userId}/{productId}")]
        public async Task<IActionResult> DeleteFavouritesByUserId(string userId, string productId)
        {
            var favouritesToDelete = await _context.Favourites
                                                    .Where(f => f.CustomerId == userId && f.Product.Id == productId)
                                                    .ToListAsync();

            if (favouritesToDelete.Count == 0)
            {
                return NotFound();
            }

            _context.Favourites.RemoveRange(favouritesToDelete);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("deleteAllFavourites")]
        public async Task<ActionResult<List<Favourite>>> DeleteAllFavourites()
        {
            _context.Favourites.RemoveRange(_context.Favourites);
            var deletedRows = await _context.SaveChangesAsync();

            return Ok(deletedRows);
        }
    }
}