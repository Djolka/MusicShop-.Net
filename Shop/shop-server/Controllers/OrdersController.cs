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
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getOrders")]
        public async Task<ActionResult<List<Order>>> GetOrders() {
            var orders = await _context.Orders.Include(o => o.Products).ToListAsync();

            if (orders == null) {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order) {
            try
            {
                foreach (var product in order.Products) {
                    _context.Attach(product);
                }
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = "An error occurred while saving the order.",
                    message = e.Message,
                    inner = e.InnerException?.Message,
                    stackTrace = e.StackTrace
                });
            }
        }

        [HttpGet("userOrders/{id}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUser(string id) {
            var orders = await _context.Orders.Where(o => o.CustomerId.Equals(id)).Include(o => o.Products).ToListAsync();
            if (orders == null) {
                return NotFound();
            }

            return Ok(orders);
        }


        [HttpDelete("deleteOrders")]
        public async Task<IActionResult> DeleteAllOrders() {
            _context.Orders.RemoveRange(_context.Orders);
            await _context.SaveChangesAsync();

            return Ok();
        }

        
    }
}