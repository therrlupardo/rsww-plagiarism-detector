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

    internal class SourceDocumentsService : EventService, ISourceDocumentsService
    {
        private static ulong _cachedVersion;
        private List<DocumentAddedToSourceEvent> _cachedEvents = new();
        private async Task<List<DocumentAddedToSourceEvent>> GetCachedOrFetch()
        {
            var current = await GetStreamRevision(EventStreams.SourceDocuments);

            if (_cachedVersion == current)
            {
                return _cachedEvents;
            }

            _cachedEvents = await GetAllEventsFromStream<DocumentAddedToSourceEvent>(EventStreams.SourceDocuments);
            _cachedVersion = current;

            return _cachedEvents;
        }

        public SourceDocumentsService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task<List<DocumentAddedToSourceEvent>> GetDocumentsAddedToSourceByAnyUserAsync() =>
            await GetCachedOrFetch();

        public async Task SaveDocumentAddedToSource(DocumentAddedToSourceEvent @event) => await SaveEvent(@event, EventStreams.SourceDocuments);
    }
}