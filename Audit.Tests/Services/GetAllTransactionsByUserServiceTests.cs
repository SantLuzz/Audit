using Audit.Application.Commands;
using Audit.Application.Services;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Domain.ValueObjects;
using Moq;

namespace Audit.Tests.Services
{
    [TestClass]
    public class GetAllTransactionsByUserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private GetAllTransactionsByUserService _service;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new GetAllTransactionsByUserService(_userRepositoryMock.Object);
        }

        [TestCategory("Serviços - Listar Transações")]
        [TestMethod]
        public async Task ExecutAsync_UserNotFound_ReturnsError()
        {
            GetAllTransactionByUserCommand command = new() { Email =  "nonexistent@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);

            var result = await _service.ExecutAsync(command);

            Assert.IsFalse(result.Success);
        }

        [TestCategory("Serviços - Listar Transações")]
        [TestMethod]
        public async Task ExecutAsync_UserWithTransactions_ReturnsTransactions()
        {

            var user = new User(new Email("user@example.com"), "Test User");
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Deposit, 1000, "teste depósito 1", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Deposit, 1500, "teste depósito 2", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Withdrawal, 100, "teste saque 1", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Withdrawal, 300, "teste saque 2", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Purchase, 550, "teste compra 1", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Purchase, 255, "teste compra 2", user.Email.Address));

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(user.Email.Address)).ReturnsAsync(user);


            GetAllTransactionByUserCommand command = new() { Email = user.Email.Address };
            var result = await _service.ExecutAsync(command);

            Assert.IsTrue(result.Success);
        }
    }
}
