using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MusicShop.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<Order?> LoadNewOrderAsync(Order newOrder)
        {
            return await _dbSet
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == newOrder.Id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _dbSet
                            .Where(o => o.CustomerId.Equals(userId))
                            .Include(o => o.OrderProducts)
                            .ThenInclude(op => op.Product)
                            .ToListAsync();
        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet
                            .Include(o => o.OrderProducts)
                            .ThenInclude(op => op.Product)
                            .ToListAsync();
        }
    }
}
