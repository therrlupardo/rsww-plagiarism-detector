using System;
using System.Threading.Tasks;
using CommandService.Extensions;
using Microsoft.AspNetCore.Http;
using OperationContracts;
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

        public async Task<Guid> SendFileToAnalysis(IFormFile file, Guid userId)
        {
            var fileId = Guid.NewGuid();
            var fileModel = file.ToFileModel();
            var command = new AddDocumentToAnalysisCommand(fileId, userId, fileModel, DateTime.Now);
            await _client.PublishAsync(command);
            return fileId;
        }

        public async Task<Guid> AnalyzeFile(Guid fileId, Guid userId)
        {
            var taskId = Guid.NewGuid();
            var command = new AnalyzeDocumentCommand(taskId, fileId, userId, DateTime.Now);
            await _client.PublishAsync(command);
            return taskId;
        }
    }
}