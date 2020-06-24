using PaymentContext.Shared.Commands;

namespace PaymentContext.Shared.Hanlers 
{
    public interface IHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }
}