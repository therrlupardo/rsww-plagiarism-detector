using System;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Utilities;
using EventStore.Client;

namespace EventsFacade.Services
{
    public interface IDocumentAnalysisService
    {
        Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId);
    }

    internal class DocumentAnalysisService : EventService, IDocumentAnalysisService
    {
        public DocumentAnalysisService(EventStoreClient storeClient) : base(storeClient) { }

        public async Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId) => await SaveEvent(@event, userId.ToUserAnalysisStreamName());
    }

}