using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnalysisService;
using Commands;
using EventsFacade;
using Google.Protobuf;
using Grpc.Net.Client;

namespace CommandHandler.Handlers
{
    public class AddDocumentToSourceStoreCommandHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;
        private readonly GrpcChannel _channel;

        public AddDocumentToSourceStoreCommandHandler(SourceDocumentFacade sourceDocumentFacade, GrpcChannel channel)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
            _channel = channel;
        }

        public async Task<Result> HandleAsync(AddDocumentToSourceStoreCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] received command {command}");

            var client = new AnalysisStart.AnalysisStartClient(_channel);
            var fileId = Guid.NewGuid();
            var req = new PersistanceRequest { FileId = fileId.ToString(), UserId = command.UserId.ToString(), FileContent = ByteString.CopyFrom(command.File.Content) };
            var reply = await client.PersistFileAsync(req);

            await _sourceDocumentFacade.SaveDocumentAddedToSource(command);

            return fileId.ToSuccessfulResult();
        }
    }
}