using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repositories;
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
using System;

namespace MusicShop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductsRepository _orderProductsRepository;
        private readonly ITransactionRepository _transactionRepository;

        public OrderController(IOrderRepository orderRepository, IOrderProductsRepository orderProductsRepository, ITransactionRepository transactionRepository)
        {
            _orderRepository = orderRepository;
            _orderProductsRepository = orderProductsRepository;
            _transactionRepository = transactionRepository;
        }

        [AllowAnonymous]
        [HttpGet("getOrders")]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {

            await _transactionRepository.BeginTransactionAsync();

            try
            {
                // Create order instance without order products
                var newOrder = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = order.CustomerId,
                    Date = order.Date,
                    TotalPrice = order.TotalPrice,
                };

                await _orderRepository.AddAsync(newOrder);
                await _orderRepository.SaveChangesAsync();

                // Create and add OrderProducts referencing the saved Order.Id
                foreach (var op in order.OrderProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = newOrder.Id,  
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    };

                    await _orderProductsRepository.AddAsync(orderProduct);
                }

                await _orderProductsRepository.SaveChangesAsync();

                await _transactionRepository.CommitAsync();

                // Load the order with products to return it
                var createdOrder = await _orderRepository.LoadNewOrderAsync(newOrder);

                if (createdOrder == null) return NotFound("Order not found after creation.");

                return Ok(createdOrder);
            }
            catch (Exception e)
            {
                await _transactionRepository.RollbackAsync();
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
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpDelete("deleteOrders")]
        public async Task<IActionResult> DeleteAllOrders() {
            await _orderRepository.DeleteAllAsync();

            return Ok();
        }        
    }
}