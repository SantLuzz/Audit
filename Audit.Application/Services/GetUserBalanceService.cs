using Audit.Application.Commands;
using Audit.Domain.Repositories;
using Audit.Shared.Notifiables;
using Audit.Shared.Services;

namespace Audit.Application.Services
{
    public class GetUserBalanceService : Notifiable, IService
    {
        private readonly IUserRepository _userRepository;
        public GetUserBalanceService(IUserRepository userRepository)
            => _userRepository = userRepository;

        public async Task<IServiceResult> ExecutAsync(GetUserBalanceCommand command)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(command.Email);
                if (user is null)
                    return new ServiceResult<decimal>(false, 0, "E104 - Usuário não cadastrado.");

                decimal balance = await _userRepository.GetBalanceAsync(command.Email);

                return new ServiceResult<decimal>(true, balance, $"Saldo do usuário: {balance.ToString("C")}");
            }
            catch (Exception)
            {
                return new ServiceResult<decimal>(false, 0, "E103 - Erro ao obter o saldo.");
            }
        }
    }
}
