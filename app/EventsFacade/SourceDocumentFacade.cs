using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Commands;
using EventsFacade.Events;
using EventsFacade.Services;

[assembly: InternalsVisibleTo("EventsFacade.Tests")]

namespace EventsFacade
{
    public class SourceDocumentFacade
    {
        private readonly ISourceDocumentsService _sourceDocumentsService;
        private readonly DocumentAnalysisService _analysisService;

        public SourceDocumentFacade(ISourceDocumentsService sourceDocumentsService)
        {
            _sourceDocumentsService = sourceDocumentsService;
        }

        public async Task<List<DocumentAddedToSourceEvent>> GetDocumentAddedToSourceEvents() =>
            await _sourceDocumentsService.GetDocumentsAddedToSourceByAnyUserAsync();

        public async Task SaveDocumentAddedToSource(AddDocumentToSourceStoreCommand command)
        {
            var @event = new DocumentAddedToSourceEvent
            {
                OccurenceDate = DateTime.Now,
                FileName = command.File.FileName,
                FileId = command.Id,
                UserId = command.UserId
            };

            await _sourceDocumentsService.SaveDocumentAddedToSource(@event);
        }
    }
}