using System.ComponentModel.DataAnnotations;

namespace Audit.Shared.Commands
{
    public abstract class Command
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
