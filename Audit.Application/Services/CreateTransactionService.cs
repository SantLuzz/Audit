using Audit.Application.Commands;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Shared.Notifiables;
using Audit.Shared.Services;

namespace Audit.Application.Services
{
    public class CreateTransactionService : Notifiable, IService
    {
        private readonly IUserRepository _userRepository;

        public CreateTransactionService(IUserRepository userRepository)
            => _userRepository = userRepository;

        public async Task<IServiceResult> ExecutAsync(CreateTransactionCommand command)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(command.Email);

                if(user is null)
                    return new ServiceResult<User>(false, null, "E109 - Usuário não cadastrado.");

                Transaction transaction = new(command.TransactionType, 
                    command.Amount, 
                    command.Description, 
                    command.Email);

                user.AddTransaction(transaction);
                
                if (!user.IsValid)
                    return new ServiceResult<User>(false, null, "E108 - " + string.Join(", ", user.Errors));

                await _userRepository.UpdateAsync(user);
                return new ServiceResult<User>(true, user, "Transação cadastrada com sucesso.");
            }
            catch (Exception)
            {
                return new ServiceResult<User>(false, null, "E107 - Erro ao criar uma nova transação.");
            }
        }
    }
}
