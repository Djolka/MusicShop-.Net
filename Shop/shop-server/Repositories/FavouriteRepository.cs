using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MusicShop.Repositories
{
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Favourite>> GetFavouritesByUserAsync(string id)
        {
            var favsByUser = await _dbSet
                                    .Include(f => f.Product)
                                    .Where(f => f.CustomerId == id)
                                    .ToListAsync();
            return favsByUser;
        }

        public override async Task AddAsync(Favourite fav) 
        {
            _context.Attach(fav.Product);
            await _dbSet.AddAsync(fav);
        }

        public async Task<bool> FindFavouriteAsync(Favourite fav)
        {
            return await _dbSet.AnyAsync(f => f.CustomerId.Equals(fav.CustomerId) && f.Product.Id.Equals(fav.Product.Id));
        }

        public async Task<IEnumerable<Favourite>> GetFavouritesByUserIdAsync(string userId, string productId)
        {
            return await _dbSet
                          .Where(f => f.CustomerId == userId && f.Product.Id == productId)
                          .ToListAsync();
        }
    }
}
