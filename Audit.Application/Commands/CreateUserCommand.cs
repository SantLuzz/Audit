using Audit.Shared.Commands;
using System.ComponentModel.DataAnnotations;

namespace Audit.Application.Commands
{
    public class CreateUserCommand : Command
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
