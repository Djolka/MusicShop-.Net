using Microsoft.AspNetCore.Mvc;
using MusicShop.Data;
using MusicShop.Models;
using MusicShop.Repositories;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("productList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);   
        }

        [HttpPost("insertMany")]
        public async Task<IActionResult> InsertProducts([FromBody] IEnumerable<Product> products)
        {
            _productRepository.InsertProducts(products);
            await _productRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("deleteAllProducts")]
        public async Task<IActionResult> DeleteAllProducts()
        {
            await _productRepository.DeleteAllAsync();
            await _productRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterProducts([FromBody] Dictionary<string, int> filter)
        {

            var filterArray = filter
                            .Where(f => f.Value == 1)
                            .Select(f => f.Key)
                            .ToList();

            var products = await _productRepository.FilterProductsAsync(filterArray);

            if (products == null) {
                return NotFound();
            }

            return Ok(products);
        }
    }
}