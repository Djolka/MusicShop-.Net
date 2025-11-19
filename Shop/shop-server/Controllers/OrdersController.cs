using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using MusicShop.Repositories;
using MusicShop.Services;
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
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MusicShop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderProductsRepository _orderProductsRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEmailSenderService _emailSenderService;

        public OrderController(IOrderRepository orderRepository, IOrderProductsRepository orderProductsRepository, ITransactionRepository transactionRepository, IEmailSenderService emailSenderService, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _orderProductsRepository = orderProductsRepository;
            _transactionRepository = transactionRepository;
            _emailSenderService = emailSenderService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            await _transactionRepository.BeginTransactionAsync();

            try
            {
                // Create order entity
                var newOrder = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = orderDto.CustomerId,
                    Date = orderDto.Date,
                    TotalPrice = orderDto.TotalPrice
                };

                await _orderRepository.AddAsync(newOrder);
                await _orderRepository.SaveChangesAsync();

                // Map OrderProducts
                foreach (var opDto in orderDto.OrderProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = newOrder.Id,
                        ProductId = opDto.ProductId!,
                        Quantity = opDto.Quantity
                    };

                    await _orderProductsRepository.AddAsync(orderProduct);
                }

                await _orderProductsRepository.SaveChangesAsync();


                // Load the order with products to return
                var createdOrder = await _orderRepository.LoadNewOrderAsync(newOrder);

                if (createdOrder == null)
                    return NotFound("Order not found after creation.");

                // Take customer email
                var user = await _userRepository.GetByIdAsync(orderDto.CustomerId);
                if(user == null)
                {
                    return BadRequest("User not found.");
                }
                string userEmail = user.Email;
                await _emailSenderService.SendEmailAsync(userEmail, "Order created", "Your order " + newOrder.Id + " is succesfully created!\n Please wait for verification of the order.");
                
                
                await _transactionRepository.CommitAsync();
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteOrders")]
        public async Task<IActionResult> DeleteAllOrders()
        {
            await _orderRepository.DeleteAllAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("verifyOrder/{orderId}")]
        public async Task<IActionResult> VerifyOrder(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Take customer email
            var user = await _userRepository.GetByIdAsync(order.CustomerId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            order.isVerified = true;

            await _orderProductsRepository.SaveChangesAsync();

            // Send Email
            try
            {
                await _emailSenderService.SendEmailAsync(
                    user.Email,
                    "Order verified",
                    $"Your order {orderId} is verified!\nYour order will arrive in 3-5 days."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }
            return Ok();
        }
    }
}