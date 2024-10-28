using Audit.Domain.Entities;
using Audit.Domain.ValueObjects;
using Audit.Infra.Data;
using Audit.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Audit.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private AppDbContext _context;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
            .Options;

            _context = new AppDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [TestCategory("Repositorios - Usuário")]
        [TestMethod]
        public async Task AddAsync_ShouldAddUser_WhenUserIsValid()
        {
            var user = new User(new Email("test@example.com"), "Test User");

            await _userRepository.AddAsync(user);

            var addedUser = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Email == user.Email.Address);

            Assert.IsNotNull(addedUser);
        }


        [TestCategory("Repositorios - Usuário")]
        [TestMethod]
        public async Task GetBalanceAsync_ShouldReturnBalance_WhenUserHasTransactions()
        {
            var user = new User(new Email("test@example.com"), "Test User");
            await _userRepository.AddAsync(user);

            _context.Transactions.Add(new Transaction(Domain.Enums.ETransactionType.Deposit, 50, "Initial Deposit", user.Email.Address));
            _context.Transactions.Add(new Transaction(Domain.Enums.ETransactionType.Withdrawal, 20, "ATM Withdrawal", user.Email.Address));
            await _context.SaveChangesAsync();

            var balance = await _userRepository.GetBalanceAsync(user.Email.Address);

            Assert.AreEqual(30, balance);
        }

        [TestCategory("Repositorios - Usuário")]
        [TestMethod]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            var user = new User(new Email("test@example.com"), "Test User");
            await _userRepository.AddAsync(user);

            var foundUser = await _userRepository.GetByEmailAsync("test@example.com");

            Assert.IsNotNull(foundUser);
        }

        [TestCategory("Repositorios - Usuário")]
        [TestMethod]
        public async Task UpdateAsync_ShouldAddTransaction_WhenTransactionDoesNotExist()
        {
            var user = new User(new Email("test@example.com"), "Test User");
            await _userRepository.AddAsync(user);

            var userToUpdate = await _userRepository.GetByEmailAsync("test@example.com");

            var newTransaction = new Transaction(Domain.Enums.ETransactionType.Deposit, 50, "New Deposit", user.Email.Address);

            userToUpdate.AddTransaction(newTransaction);

            await _userRepository.UpdateAsync(userToUpdate);

            var addedTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.UserId == user.Email.Address && t.Id == newTransaction.Id);

            Assert.IsNotNull(addedTransaction);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
