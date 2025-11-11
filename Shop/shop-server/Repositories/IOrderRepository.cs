using System.Threading.Tasks;
using System.Collections.Generic;
using MusicShop.Models;

namespace MusicShop.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> LoadNewOrderAsync(Order newOrder);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
