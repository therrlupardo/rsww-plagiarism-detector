using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Services;
using OperationContracts.Enums;

[assembly: InternalsVisibleTo("EventsFacade.Tests")]

namespace EventsFacade
{
    public class SourceDocumentFacade
    {
        private readonly ISourceDocumentsService _sourceDocumentsService;


        public SourceDocumentFacade(ISourceDocumentsService sourceDocumentsService)
        {
            _sourceDocumentsService = sourceDocumentsService;
        }

        public async Task<List<DocumentAddedToSourceEvent>> GetDocumentAddedToSourceEvents() =>
            await _sourceDocumentsService.GetDocumentsAddedToSourceByAnyUserAsync();

        public async Task SaveDocumentAddedToSource(string fileName, Guid fileId, Guid userId, OperationStatus status)
        {
            var @event = new DocumentAddedToSourceEvent
            {
                OccurenceDate = DateTime.Now,
                FileName = fileName,
                FileId = fileId,
                UserId = userId,
                Status = status
            };

            await _sourceDocumentsService.SaveDocumentAddedToSource(@event);
        }
    }
}