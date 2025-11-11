using System.Threading.Tasks;
using System.Collections.Generic;
using MusicShop.Models;

namespace MusicShop.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> FilterProductsAsync(IEnumerable<string> types);
        void InsertProducts(IEnumerable<Product> products);
    }
}
