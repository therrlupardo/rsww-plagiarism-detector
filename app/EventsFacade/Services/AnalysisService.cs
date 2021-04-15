using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Utilities;
using EventStore.Client;

namespace EventsFacade.Services
{
    public interface IDocumentAnalysisService
    {
        Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId);
        Task<List<DocumentAnalysisStatusChangedEvent>> GetAnalysesForUser(Guid userId);
    }

    internal class DocumentAnalysisService : EventService, IDocumentAnalysisService
    {
        private static ulong _cachedVersion;
        private List<DocumentAnalysisStatusChangedEvent> _cachedEvents = new();
        private async Task<List<DocumentAnalysisStatusChangedEvent>> GetCachedOrFetch(Guid userId)
        {
            var stream = userId.ToUserAnalysesStreamName();
            var current = await GetStreamRevision(stream);

            if (_cachedVersion == current)
            {
                return _cachedEvents;
            }

            _cachedEvents = await GetAllEventsFromStream<DocumentAnalysisStatusChangedEvent>(userId.ToUserAnalysesStreamName());
            _cachedVersion = current;

            return _cachedEvents;
        }
        public DocumentAnalysisService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId) => await SaveEvent(@event, userId.ToUserAnalysesStreamName());

        public async Task<List<DocumentAnalysisStatusChangedEvent>> GetAnalysesForUser(Guid userId) =>
            await GetCachedOrFetch(userId);
    }

}