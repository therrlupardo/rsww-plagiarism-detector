using System;
using System.Threading.Tasks;
using Commands;
using CommandService.Extensions;
using Microsoft.AspNetCore.Http;
using RawRabbit;

namespace CommandService.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IBusClient _client;

        public AnalysisService(IBusClient client)
        {
            _client = client;
        }

        public async Task<Guid> PerformAnalysis(IFormFile file, Guid userId)
        {
            var taskId = Guid.NewGuid();
            var fileModel = file.ToFileModel();
            var command = new VerifyDocumentCommand(taskId, userId, fileModel);
            await _client.PublishAsync(command);
            return taskId;
        }
    }
}