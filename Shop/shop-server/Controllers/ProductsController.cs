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
using Microsoft.AspNetCore.Authorization;

namespace MusicShop.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("productList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);   
        }

        [HttpPost("insertMany")]
        public async Task<IActionResult> InsertProducts([FromBody] IEnumerable<Product> products)
        {
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("deleteAllProducts")]
        public async Task<IActionResult> DeleteAllProducts()
        {
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterProducts([FromBody] Dictionary<string, int> filter)
        {

            var filterArray = new List<string>();
            foreach (var field in filter) {
                if (field.Value == 1) { 
                    filterArray.Add(field.Key);
                }
            }

            var products = await _context.Products
                                            .Where(elem => filterArray.Contains(elem.Type))
                                            .ToListAsync();

            if (products == null) {
                return NotFound();
            }

            return Ok(products);
        }
    }
}