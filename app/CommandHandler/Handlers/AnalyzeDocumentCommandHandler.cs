using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EventsFacade;
using Microsoft.Extensions.Configuration;
using OperationContracts;
using OperationContracts.Enums;

namespace CommandHandler.Handlers
{
    public class AnalyzeDocumentCommandHandler : IHandler<AnalyzeDocumentCommand>
    {
        private readonly AnalysisFacade _analyzeEventsFacade;
        private readonly IConfiguration _configuration;

        public AnalyzeDocumentCommandHandler(AnalysisFacade analyzeEventsFacade, IConfiguration configuration)
        {
            _analyzeEventsFacade = analyzeEventsFacade;
            _configuration = configuration;
        }

        public async Task<Result> HandleAsync(AnalyzeDocumentCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(AnalyzeDocumentCommandHandler)} received command {command} ");

            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, command.TaskId, command.IssuedTime, command.UserId, OperationStatus.Running, null);

            // TODO: Perform the analysis here
            var result = PerformAnalysis();
            //TODO: Wrap it in a service and send the notification to the frontend
            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.TaskId, command.IssuedTime,
                command.UserId, OperationStatus.Complete, result);

            return Result.Success();
        }

        private string PerformAnalysis()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = _configuration.GetSection("analysisScript").Value,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}