namespace Audit.Shared.Notifiables
{
    public abstract class Notifiable
    {
        private List<string> _errors = new List<string>();

        public bool IsValid => !_errors.Any();
        public IReadOnlyCollection<string> Errors => _errors;

        public void AddError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) return;

            _errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            if (!errors?.Any() ?? false)
                return;
                
            _errors.AddRange(errors);
        }

        public void AddErrors(params Notifiable[] errors)
        {
            foreach (Notifiable erro in errors)
                _errors.AddRange(erro.Errors);
        }
    }
}
