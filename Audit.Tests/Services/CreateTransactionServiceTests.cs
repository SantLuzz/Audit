using Audit.Application.Commands;
using Audit.Application.Services;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Moq;

namespace Audit.Tests.Services
{
    [TestClass]
    public class CreateTransactionServiceTests
    {
        private User _user;
        private Mock<IUserRepository> _userRepositoryMock;
        private CreateTransactionService _service;
        private CreateTransactionCommand _command;

        public CreateTransactionServiceTests()
        {
            _user = new User(new Domain.ValueObjects.Email("batman@wayne.com"), "batman");
            _command = new Application.Commands.CreateTransactionCommand
            {
                TransactionType = Domain.Enums.ETransactionType.Deposit,
                Amount = 100,
                Description = "teste de deposito",
                Email = _user.Email.Address
            };
        }

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new CreateTransactionService(_userRepositoryMock.Object);
        }

        [TestCategory("Serviços - Criação de Transação")]
        [TestMethod]
        public async Task ExecutAsync_UserNotFound_ReturnsError()
        {
            var command = new Application.Commands.CreateTransactionCommand
            {
                TransactionType = Domain.Enums.ETransactionType.Deposit,
                Amount = 100,
                Description = "teste de deposito",
                Email = "nonexistent@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);

            var result = await _service.ExecutAsync(command);
            Assert.IsFalse(result.Success);
        }

        [TestCategory("Serviços - Criação de Transação")]
        [TestMethod]
        public async Task ExecutAsync_UserValid_TransactionSuccess()
        {
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(_command.Email)).ReturnsAsync(_user);

            var result = await _service.ExecutAsync(_command);
            Assert.IsTrue(result.Success);
        }
    }
}
