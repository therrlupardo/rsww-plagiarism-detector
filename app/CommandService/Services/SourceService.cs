using System;
using System.Threading.Tasks;
using Commands;
using CommandService.Extensions;
using Microsoft.AspNetCore.Http;
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
            var command = new AddDocumentToSourceStoreCommand(taskId, userId, fileModel);
            await _client.PublishAsync(command);
            return taskId;
        }
    }
}