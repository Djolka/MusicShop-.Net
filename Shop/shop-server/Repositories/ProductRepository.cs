using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicShop.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> FilterProductsAsync(IEnumerable<string> types)
        {
            return await _dbSet
                            .Where(p => types.Contains(p.Type))
                            .ToListAsync();
        }

        public void InsertProducts(IEnumerable<Product> products) 
        {
            _context.Products.AddRange(products);
        }
    }
}
