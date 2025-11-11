using System.Threading.Tasks;
using System.Collections.Generic;
using MusicShop.Models;

namespace MusicShop.Repositories
{
    public interface IFavouriteRepository : IGenericRepository<Favourite>
    {
        Task<IEnumerable<Favourite>> GetFavouritesByUserAsync(string id);
        Task<bool> FindFavouriteAsync(Favourite fav);
        Task<IEnumerable<Favourite>> GetFavouritesByUserIdAsync(string userId, string productId);
    }
}
