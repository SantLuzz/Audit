using Audit.Domain.Enums;
using Audit.Domain.ValueObjects;
using Audit.Shared.Entities;
using System.Text.RegularExpressions;

namespace Audit.Domain.Entities;
public class User : Entity
{
    private List<Transaction> _transactions = new List<Transaction>();
    public Email Email { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Transaction> Transactions => _transactions;

    public User(Email email, string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            AddError("Nome é obrigatório");

        if (email is null)
            AddError("E-mail é obrigatório");
        else
            AddErrors(email);

        this.Email = email;
        this.Name = name;
        this.CreatedAt = DateTime.Now;
        this.Balance = 0;
    }

    public User(Email email, string name, decimal balance, DateTime creatAt) 
        : this(email, name)
    {
        this.CreatedAt = creatAt;
        this.Balance = balance;
    }

    public void AddTransaction(Transaction transaction)
    {
        if (!transaction.IsValid)
        {
            this.AddErrors(transaction.Errors);
            return;
        }

        // Verificar o tipo da transação
        switch (transaction.TransactionType)
        {
            case ETransactionType.Deposit:
                // Adicionar saldo
                AddBalance(transaction.Amount);
                break;

            case ETransactionType.Withdrawal:
            case ETransactionType.Purchase:
                // Validar se há saldo suficiente
                if (Balance < transaction.Amount)
                {
                    AddError("Saldo insuficiente para a transação");
                    return;
                }
                // Subtrair saldo
                SubtractBalance(transaction.Amount);
                break;

            default:
                AddError("Tipo de transação inválido");
                return;
        }

        // Adicionar a transação à lista de transações do usuário
        _transactions.Add(transaction);
    }

    public void AddTransactions(IEnumerable<Transaction> transactions)
    {
        foreach (var transaction in transactions)
            AddTransaction(transaction);
    }

    private void AddBalance(decimal amount)
    {
        if (amount <= 0)
        {
            AddError("O valor do depósito deve ser maior que zero");
            return;
        }

        Balance += amount;
    }

    private void SubtractBalance(decimal amount)
    {
        if (amount <= 0)
        {
            AddError("O valor da retirada deve ser maior que zero");
            return;
        }

        if (amount > this.Balance)
        {
            AddError("Saldo Insuficiente. O valor não pode ser maior que o saldo atual.");
            return;
        }

        Balance -= amount;
    }

    
}
