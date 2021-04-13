using System.Threading;
using System.Threading.Tasks;
using OperationContracts;

namespace CommandHandler.Handlers
{
    public interface IHandler<in TCommand> where TCommand : BaseCommand
    {
        Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }

    public interface IHandler<in TCommand, TResult> where TCommand : BaseCommand
    {
        Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}