using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void AdicionarAssinatura()
        {
            var subscription = new Subscription(null);

            var student = new Student(
                "Othon", 
                "Godoy", 
                "12345678901", 
                "othon.email.com"
            );

            student.AddSubscription(subscription);
        }
    }
}
