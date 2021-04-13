using System;
using System.Threading.Tasks;
using CommandService.Extensions;
using Microsoft.AspNetCore.Http;
using OperationContracts;
using RawRabbit;

namespace CommandService.Services
{
    public class SourceService : ISourceService
    {
        private readonly IBusClient _client;

        public SourceService(IBusClient client)
        {
            _client = client;
        }

        public async Task<Guid> CreateSource(IFormFile file, Guid userId)
        {
            var taskId = Guid.NewGuid();
            var fileModel = file.ToFileModel();
            var command = new AddDocumentToSourceStoreCommand(taskId, userId, DateTime.Now, fileModel);
            await _client.PublishAsync(command);
            return taskId;
        }
    }
}