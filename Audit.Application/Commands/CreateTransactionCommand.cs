using Audit.Domain.Enums;
using Audit.Shared.Commands;
using System.ComponentModel.DataAnnotations;

namespace Audit.Application.Commands
{
    public class CreateTransactionCommand : Command
    {
        [Required]
        public ETransactionType TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
