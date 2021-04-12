using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Utilities;
using EventStore.Client;

namespace EventsFacade.Services
{
    public interface IDocumentsToAnalysisService
    {
        Task SaveDocumentToAnalysisAddedEvent(DocumentToAnalysisAddedEvent @event);
        Task<List<DocumentToAnalysisAddedEvent>> GetAllUserDocumentsAddedToAnalysis(Guid userId);
    }

    internal class DocumentsToAnalysisService : EventService, IDocumentsToAnalysisService
    {
        public DocumentsToAnalysisService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task<List<DocumentToAnalysisAddedEvent>> GetAllUserDocumentsAddedToAnalysis(Guid userId) =>
            await GetAllEventsFromStream<DocumentToAnalysisAddedEvent>(userId.ToUserDocumentsToAnalysisStreamName());

        public async Task SaveDocumentToAnalysisAddedEvent(DocumentToAnalysisAddedEvent @event) =>
            await SaveEvent(@event, @event.UserId.ToUserDocumentsToAnalysisStreamName());
    }
}