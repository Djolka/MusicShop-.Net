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
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                // Step 1: Create order instance without order products
                var newOrder = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = order.CustomerId,
                    Date = order.Date,
                    TotalPrice = order.TotalPrice,
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync(); // Save to get the OrderId persisted

                // Step 2: Create and add OrderProducts referencing the saved Order.Id
                foreach (var op in order.OrderProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = newOrder.Id,  // Now you have a valid orderId
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    };

                    _context.OrderProducts.Add(orderProduct); // Assuming you have DbSet<OrderProduct> OrderProducts
                }

                await _context.SaveChangesAsync(); // Save all order products

                // Optionally, load the order with products to return it
                var createdOrder = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == newOrder.Id);

                return Ok(createdOrder);
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




        [HttpGet("userOrders/{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrderProductsByUser(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId.Equals(userId))
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
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