using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class CreateBoletoSubscriptionCommandTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenNameIsValid()
        {
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "";

            command.Validate();

            Assert.AreEqual(false, command.Valid);
        }             
    }
}
