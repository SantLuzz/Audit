using Audit.Domain.ValueObjects;

namespace Audit.Tests.ValueObjects
{
    [TestClass]
    public class EmailTests
    {
        private readonly string _adress = "teste@teste.com";

        [TestCategory("Value Objects")]
        [TestMethod]
        public void Should_return_error_when_email_is_empty()
        {
            var email = new Email("");
            Assert.IsFalse(email.IsValid);
        }

        [TestCategory("Value Objects")]
        [TestMethod]
        public void Should_return_error_when_email_is_invalid()
        {
            var email = new Email("invalid");
            Assert.IsFalse(email.IsValid);
        }

        [TestCategory("Value Objects")]
        [TestMethod]
        public void Should_return_success_When_Email_Is_Valid()
        {
            var email = new Email(_adress);
            Assert.IsTrue(email.IsValid);
        }

        [TestCategory("Value Objects")]
        [TestMethod]
        public void Should_return_error_when_email_does_not_have_at_symbol()
        {
            var email = new Email("userexample.com");
            Assert.IsFalse(email.IsValid);
        }

        [TestCategory("Value Objects")]
        [TestMethod]
        public void Should_return_error_when_email_does_not_have_domain()
        {
            var email = new Email("user@");
            Assert.IsFalse(email.IsValid);
        }
    }
}
