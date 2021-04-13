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
        public DocumentAnalysisService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId) => await SaveEvent(@event, userId.ToUserAnalysesStreamName());

        public async Task<List<DocumentAnalysisStatusChangedEvent>> GetAnalysesForUser(Guid userId) => await GetAllEventsFromStream<DocumentAnalysisStatusChangedEvent>(userId.ToUserAnalysesStreamName());
    }

}