using Audit.Domain.Entities;

namespace Audit.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<decimal> GetBalanceAsync(string email);
    }
}
