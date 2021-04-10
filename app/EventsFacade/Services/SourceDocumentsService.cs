using System.Collections.Generic;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Utilities;
using EventStore.Client;

namespace EventsFacade.Services
{
    public interface ISourceDocumentsService
    {
        Task<List<DocumentAddedToSourceEvent>> GetDocumentsAddedToSourceByAnyUserAsync();
        Task SaveDocumentAddedToSource(DocumentAddedToSourceEvent command);
    }

    public class SourceDocumentsService : EventService, ISourceDocumentsService
    {
        public SourceDocumentsService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task<List<DocumentAddedToSourceEvent>> GetDocumentsAddedToSourceByAnyUserAsync() => await GetAllEventsFromStream<DocumentAddedToSourceEvent>(EventStreams.SourceDocuments);

        public async Task SaveDocumentAddedToSource(DocumentAddedToSourceEvent @event) => await SaveEvent(@event, EventStreams.SourceDocuments);
    }
}