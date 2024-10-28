using Audit.Application.Commands;
using Audit.Application.Services;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Domain.ValueObjects;
using Moq;

namespace Audit.Tests.Services
{
    [TestClass]
    public class GetUserBalanceServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private GetUserBalanceService _service;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new GetUserBalanceService(_userRepositoryMock.Object);
        }

        [TestMethod]
        public async Task ExecutAsync_UserNotFound_ReturnsError()
        {
            var command = new GetUserBalanceCommand { Email = "nonexistent@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);


            var result = await _service.ExecutAsync(command);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task ExecutAsync_ValidUser_ReturnsBalance()
        {
            var user = new User(new Email("user@example.com"), "Test User");
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Deposit, 1000, "teste depósito 1", user.Email.Address));
            user.AddTransaction(new Transaction(Domain.Enums.ETransactionType.Withdrawal, 100, "teste saque 1", user.Email.Address));

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(user.Email.Address)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetBalanceAsync(user.Email.Address)).ReturnsAsync(user.Balance);

            var command = new GetUserBalanceCommand { Email = user.Email.Address };
            var result = await _service.ExecutAsync(command);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(900, user.Balance);
        }
    }
}
