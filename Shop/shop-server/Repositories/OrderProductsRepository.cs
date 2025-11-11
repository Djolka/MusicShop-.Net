using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MusicShop.Repositories
{
    public class OrderProductsRepository : GenericRepository<OrderProduct>, IOrderProductsRepository
    {
        public OrderProductsRepository(AppDbContext context) : base(context) { }
    }
}
