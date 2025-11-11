using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

public interface ITransactionRepository
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
