using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();

            command.FirstName ="Bruce";
            command.LastName ="Wayne";
            command.Document = "12345678901";
            command.Email = "hello@email.com";
            command.BarCode = "123456789";
            command.BoletoNumber = "12314567890";
            command.PaymentNumber = "123321";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Wayne Corp";
            command.PayerDocument = "12345678900";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "batman@dc.com";
            command.Street = "Street";
            command.Number = "0123";
            command.Neighborhood = "Neighborhood";
            command.City= "City";
            command.State= "State";
            command.Country= "Country";
            command.ZipCode= "ZipCode";

            handler.Handle(command);

            Assert.AreEqual(false, handler.Valid);
    }
    }
}
