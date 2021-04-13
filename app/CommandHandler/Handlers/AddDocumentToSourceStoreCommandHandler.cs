using System;
using System.Threading;
using System.Threading.Tasks;
using EventsFacade;
using OperationContracts;

namespace CommandHandler.Handlers
{
    public class AddDocumentToSourceStoreCommandHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;

        public AddDocumentToSourceStoreCommandHandler(SourceDocumentFacade sourceDocumentFacade)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
        }

        public async Task<Result> HandleAsync(AddDocumentToSourceStoreCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] received command {command}");

            await _sourceDocumentFacade.SaveDocumentAddedToSource(command.File.FileName, command.FileId, command.UserId);

            return Guid.NewGuid().ToSuccessfulResult();
        }
    }
}