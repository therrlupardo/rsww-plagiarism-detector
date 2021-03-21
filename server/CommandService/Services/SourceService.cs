using System;
using System.IO;
using System.Threading.Tasks;
using Commands;
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
            var fileModel = await ConvertToFileModel(file);
            var command = new AddDocumentToSourceStoreCommand(taskId, userId, fileModel);
            await _client.PublishAsync(command);
            return taskId;
        }

        private async Task<FileModel> ConvertToFileModel(IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return new FileModel(file.FileName, ms.ToArray());
        }
    }
}