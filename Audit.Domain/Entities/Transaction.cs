using Audit.Domain.Enums;
using Audit.Shared.Entities;

namespace Audit.Domain.Entities;

public class Transaction : Entity
{
    public ETransactionType TransactionType { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public string UserId { get; private set; }

    public Transaction(ETransactionType transactionType, decimal amount, string description, string userId)
    {
        if (amount <= 0)
            AddError("A quantia paga deve ser maior que zero");

        if(string.IsNullOrWhiteSpace(userId))
            AddError("O usuário é obrigatório");

        TransactionType = transactionType;
        Amount = amount;
        Description = description;
        Date = DateTime.Now;
        UserId = userId;
    }

    public Transaction(ETransactionType transactionType, decimal amount, string description, string userId, DateTime date)
        : this(transactionType, amount, description, userId)
    {
        Date = date;
    }
}
