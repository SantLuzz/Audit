using Audit.Application.Commands;
using Audit.Application.Services;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Domain.ValueObjects;
using Moq;

namespace Audit.Tests.Services
{
    [TestClass]
    public class CreateUserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private CreateUserService _service;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new CreateUserService(_userRepositoryMock.Object);
        }

        [TestCategory("Serviços - Criação de Usuário")]
        [TestMethod]
        public async Task ExecutAsync_UserAlreadyExists_ReturnsError()
        {
            CreateUserCommand command = new CreateUserCommand
            {
                Email = "teste@teste.com",
                Name = "Existing User",
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync(new User(new Email(command.Email), command.Name));

            var result = await _service.ExecutAsync(command);
            Assert.IsFalse(result.Success);
        }

        [TestCategory("Serviços - Criação de Usuário")]
        [TestMethod]
        public async Task ExecutAsync_ValidUser_CreatesUser()
        {
            CreateUserCommand command = new CreateUserCommand
            {
                Email = "newuser@example.com",
                Name = "New User",
            };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);

            var result = await _service.ExecutAsync(command);
            Assert.IsTrue(result.Success);
        }
    }
}
