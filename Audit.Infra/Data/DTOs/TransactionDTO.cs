using Audit.Domain.Entities;
using Audit.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.Infra.Data.DTOs
{
    [Table("Transactions")]
    public class TransactionDTO
    {
        [Key]
        public Guid Id { get; set; }
        public ETransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public UserDTO? User { get; set; }

        public static implicit operator TransactionDTO(Transaction transaction)
            => new TransactionDTO
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Description = transaction.Description,
                Date = transaction.Date,
                UserId = transaction.UserId,
            };
    }
}
