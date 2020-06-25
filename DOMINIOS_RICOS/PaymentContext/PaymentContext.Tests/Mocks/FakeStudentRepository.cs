using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Tests.Mocks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {
            
        }

        public bool DocumentExists(string document)
        {
            if (document == "12345678901")
                return true;

            return false;
        }

        public bool EmailExists(string email)
        {
            if (email == "hello@email.com")
                return true;

            return false;
        }
    }
}
