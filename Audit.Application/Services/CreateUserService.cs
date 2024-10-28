using Audit.Application.Commands;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Shared.Notifiables;
using Audit.Shared.Services;

namespace Audit.Application.Services
{
    public class CreateUserService : Notifiable, IService
    {
        private readonly IUserRepository _userRepository;

        public CreateUserService(IUserRepository userRepository) 
            => _userRepository = userRepository;

        public async Task<IServiceResult> ExecutAsync(CreateUserCommand command)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(command.Email);

                if (existingUser is not null)
                    return new ServiceResult<User>(false, null, "E101 - Usuário já cadastrado.");

                User user = new(new Domain.ValueObjects.Email(command.Email), command.Name);

                AddErrors(user);

                if(!IsValid)
                    return new ServiceResult<User>(false, null,"E102 - Não foi possível cadastrar o usuário.");

                await _userRepository.AddAsync(user);
                return new ServiceResult<User>(true, user, "Usuário cadastrado com sucesso.");
            }
            catch (Exception)
            {
                return new ServiceResult<User>(false, null,$"E100 - Erro ao criar o usuário.");
            }
        }
    }
}
