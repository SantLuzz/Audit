using Audit.Domain.Entities;
using Audit.Domain.Enums;
using Audit.Domain.Repositories;
using Audit.Domain.ValueObjects;
using Audit.Infra.Data;
using Audit.Infra.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Audit.Infra.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            try
            {
                UserDTO dto = user;
                await context.Users.AddAsync(dto);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetBalanceAsync(string email)
        {
            try
            {
                var balance = await context
                    .Transactions
                    .AsNoTracking()
                    .Where(x => x.UserId.Equals(email))
                    .GroupBy(x => x.TransactionType)
                    .Select(g => new
                    {
                        Type = g.Key,
                        TotalAmount = g.Sum(x => x.Amount)
                    })
                    .ToListAsync();

                decimal totalDeposits = balance.Where(b => b.Type == ETransactionType.Deposit).Sum(b => b.TotalAmount);
                decimal totalWithdrawals = balance.Where(b => b.Type == ETransactionType.Withdrawal).Sum(b => b.TotalAmount);
                decimal totalPurchases = balance.Where(b => b.Type == ETransactionType.Purchase).Sum(b => b.TotalAmount);

  
                decimal finalBalance = totalDeposits - (totalWithdrawals + totalPurchases);
                return finalBalance;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .Include(x => x.Transactions)
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (user is null) return null;

                User domainUser = new User(new Email(user.Email),
                    user.Name, user.Balance, user.CreatedAt);

                if (user.Transactions.Any())
                {
                    IEnumerable<Transaction> transactions = user.Transactions
                        .Select(x => new Transaction(x.TransactionType, x.Amount, x.Description, x.UserId, x.Date))
                        .ToList();

                    domainUser.AddTransactions(transactions);
                }

                return domainUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {

                var existingUser = await context.Users
                    .Include(u => u.Transactions)
                    .FirstOrDefaultAsync(u => u.Email == user.Email.Address);

                if(existingUser is null)
                    throw new Exception("Usuário não encontrado.");
                
                existingUser.Balance = user.Balance;
                existingUser.Name = user.Name;

                context.Update(existingUser);

                var existingTransactionIds = existingUser
                    .Transactions
                    .Select(t => t.Id)
                    .ToList();

                foreach (var transaction in user.Transactions)
                {
                    if (!existingTransactionIds.Contains(transaction.Id))
                        context.Transactions.Add((TransactionDTO)transaction);
                }
                
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
