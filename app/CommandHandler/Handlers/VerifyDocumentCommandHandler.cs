using System;
using System.Threading;
using System.Threading.Tasks;
using AnalysisService;
using Commands;
using Grpc.Net.Client;

namespace CommandHandler.Handlers
{
    public class VerifyDocumentCommandHandler : IHandler<AddDocumentToAnalysisCommand>
    {
        private readonly GrpcChannel _channel;

        public VerifyDocumentCommandHandler(GrpcChannel channel)
        {
            _channel = channel;
        }

        public async Task<Result> HandleAsync(AddDocumentToAnalysisCommand toAnalysisCommand, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyDocumentCommandHandler] received command {toAnalysisCommand}");

            var client = new AnalysisStart.AnalysisStartClient(_channel);
            var req = new AnalysisRequest { FileId = toAnalysisCommand.Id.ToString() };
            var reply = await client.StartAnalysisAsync(req);

            return Result.Success();
        }
    }
}