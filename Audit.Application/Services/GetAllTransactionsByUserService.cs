using Audit.Application.Commands;
using Audit.Domain.Entities;
using Audit.Domain.Repositories;
using Audit.Shared.Notifiables;
using Audit.Shared.Services;

namespace Audit.Application.Services
{
    public class GetAllTransactionsByUserService : Notifiable, IService
    {
        private readonly IUserRepository _userRepository;

        public GetAllTransactionsByUserService(IUserRepository userRepository)
            => _userRepository = userRepository;
        

        public async Task<IServiceResult> ExecutAsync(GetAllTransactionByUserCommand command)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(command.Email);
                if (user is null)
                    return new ServiceResult<List<Transaction>>(false, null, "E106 - Usuário não cadastrado.");

                return new ServiceResult<IReadOnlyCollection<Transaction>>(true, user.Transactions, "Transações encontradas.");
            }
            catch (Exception)
            {
                return new ServiceResult<IReadOnlyCollection<Transaction>>(false, null, "E105 - Erro ao obter as transações do usuário.");
            }
        }
    }
}
