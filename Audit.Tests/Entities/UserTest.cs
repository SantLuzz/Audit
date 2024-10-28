using Audit.Domain.Entities;
using Audit.Domain.Enums;
using Audit.Domain.ValueObjects;

namespace Audit.Tests.Entities
{
    [TestClass]
    public class UserTest
    {
        Email _email = new Email("teste@teste.com");

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_error_when_empty_name()
        {
            User user = new User(_email, "");
            Assert.IsFalse(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_error_when_email_is_invalid()
        {
            var user = new User(new Email(""), "Valid Name");
            Assert.IsFalse(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_sucess_when_adding_user_valid()
        {
            User user = new User(_email, "teste");
            Assert.IsTrue(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_error_when_adding_deposit_with_user_id_is_empty()
        {
            User user = new User(_email, "teste");
            Transaction transaction = new Transaction(Domain.Enums.ETransactionType.Deposit, 0, "Teste deposito", "");
            user.AddTransaction(transaction);
            Assert.IsFalse(user.IsValid);
        }


        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_error_when_adding_deposit_with_zero_value_to_the_balance()
        {
            User user = new User(_email, "teste");
            Transaction transaction = new Transaction(Domain.Enums.ETransactionType.Deposit, 0, "Teste deposito", _email.Address);
            user.AddTransaction(transaction);
            Assert.IsFalse(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_return_error_when_adding_deposit_with_less_than_zero_value_to_the_balance()
        {
            User user = new User(_email, "teste");
            Transaction transaction = new Transaction(Domain.Enums.ETransactionType.Deposit, -10, "Teste deposito", _email.Address);
            user.AddTransaction(transaction);
            Assert.IsFalse(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_add_deposit_transaction_and_update_balance()
        {
            User user = new User(_email, "teste");
            Transaction transaction = new Transaction(Domain.Enums.ETransactionType.Deposit, 50, "Teste deposito", _email.Address);
            user.AddTransaction(transaction);
            Assert.AreEqual(50, user.Balance);
        }


        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_add_withdrawal_transaction_and_update_balance()
        {
            User user = new User(_email, "teste");
            user.AddTransaction(new Transaction(ETransactionType.Deposit, 200, "teste de deposito", _email.Address));

            var transaction = new Transaction(ETransactionType.Withdrawal, 50, "teste de compra", _email.Address);
            user.AddTransaction(transaction);

            Assert.AreEqual(150, user.Balance);
        }


        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_not_add_withdrawal_transaction_when_insufficient_balance()
        {
            User user = new User(_email, "teste");
            user.AddTransaction(new Transaction(ETransactionType.Deposit, 200, "teste de deposito", _email.Address));

            var transaction = new Transaction(ETransactionType.Withdrawal, 250, "teste de compra", _email.Address);
            user.AddTransaction(transaction);

            Assert.IsFalse(user.IsValid);
        }

        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_add_purchase_transaction_and_update_balance()
        {
            User user = new User(_email, "teste");
            user.AddTransaction(new Transaction(ETransactionType.Deposit, 200, "teste de deposito", _email.Address));

            var transaction = new Transaction(ETransactionType.Purchase, 50, "teste de compra", _email.Address);
            user.AddTransaction(transaction);

            Assert.AreEqual(150, user.Balance);
        }


        [TestCategory("Entidades")]
        [TestMethod]
        public void Should_not_add_purchase_transaction_when_insufficient_balance()
        {
            User user = new User(_email, "teste");
            user.AddTransaction(new Transaction(ETransactionType.Deposit, 200, "teste de deposito", _email.Address));

            var transaction = new Transaction(ETransactionType.Purchase, 250, "teste de compra", _email.Address);
            user.AddTransaction(transaction);

            Assert.IsFalse(user.IsValid);
        }
    }
}
