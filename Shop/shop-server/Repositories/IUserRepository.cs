using System.Threading.Tasks;
using System.Collections.Generic;
using MusicShop.Models;

namespace MusicShop.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
