using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Hanlers;
using System;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : 
        Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>, 
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "N�o foi poss�vel realizar sua assinatura");
            }

            // Verificar se documento j� est� cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF j� est� em uso");

            // Verificar se email j� est� cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este email j� est� em uso");

            // Gerar os VOs 
            var name = new Name(command.FirstName, command.LastName);
            var email = new Email(command.Email);
            var document = new Document(command.Document, EDocumentType.CPF);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar as valida��es
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notifica��es
            if (Invalid)
                return new CommandResult(false, "N�o foi poss�vel realizar sua assinatura");
            
            // Salvar as informa��es
            _repository.CreateSubscription(student);

            // Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            // Retornar informa��es
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            // Verificar se documento j� est� cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF j� est� em uso");

            // Verificar se email j� est� cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este email j� est� em uso");

            // Gerar os VOs 
            var name = new Name(command.FirstName, command.LastName);
            var email = new Email(command.Email);
            var document = new Document(command.Document, EDocumentType.CPF);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar as valida��es
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Salvar as informa��es
            _repository.CreateSubscription(student);

            // Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            // Retornar informa��es
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}