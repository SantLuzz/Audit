using Audit.Shared.ValueObjects;
using System.Text.RegularExpressions;

namespace Audit.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            if (!ValidateEmail(address))
                AddError("E-mail é obrigatório");

            Address = address;
        }

        public string Address { get; private set; }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
