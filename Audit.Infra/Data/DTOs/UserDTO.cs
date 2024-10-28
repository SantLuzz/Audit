using Audit.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.Infra.Data.DTOs
{
    [Table("Users")]
    public class UserDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;
        public string Name { get;  set; } = string.Empty;
        public decimal Balance { get;  set; }
        public DateTime CreatedAt { get; set; }

        public List<TransactionDTO> Transactions { get; set; } = [];

        public static implicit operator UserDTO(User user)
            => new UserDTO
            {
                Id = user.Id,
                Email = user.Email.Address,
                Name = user.Name,
                Balance = user.Balance,
                CreatedAt = user.CreatedAt,
                Transactions = user.Transactions.Select(x => (TransactionDTO)x).ToList()
            };

    }
}
