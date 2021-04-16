using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
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
        private static readonly List<DocumentAddedToSourceEvent> CachedEvents = new();
        private static bool _wasInitialized;

        private async Task<List<DocumentAddedToSourceEvent>> GetCachedOrFetch()
        {
            if (_wasInitialized) return CachedEvents;

            await ResubscribeAsync(StreamPosition.Start);

            return CachedEvents;
        }

        private async Task ResubscribeAsync(StreamPosition position)
        {
            await _storeClient.SubscribeToStreamAsync(EventStreams.SourceDocuments,
                eventAppeared: (_, e, _) =>
                {
                    var toDeserialize = Encoding.UTF8.GetString(e.Event.Data.ToArray());
                    var deserialized = JsonSerializer.Deserialize<DocumentAddedToSourceEvent>(toDeserialize);
                    CachedEvents.Add(deserialized);
                    _wasInitialized = true;

                    position = e.OriginalEventNumber;

                    return Task.CompletedTask;
                },
                subscriptionDropped: (async (subscription, reason, arg3) =>
                {
                    Debug.WriteLine("Source documents subscription dropped");
                    if (reason != SubscriptionDroppedReason.Disposed)
                    {
                        await ResubscribeAsync(position);
                    }
                }));
        }


        public SourceDocumentsService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task<List<DocumentAddedToSourceEvent>> GetDocumentsAddedToSourceByAnyUserAsync() =>
            await GetCachedOrFetch();

        public async Task SaveDocumentAddedToSource(DocumentAddedToSourceEvent @event) =>
            await SaveEvent(@event, EventStreams.SourceDocuments);
    }
}