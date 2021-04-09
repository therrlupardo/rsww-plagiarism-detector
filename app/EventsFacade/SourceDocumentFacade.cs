using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Commands;
using EventsFacade.Events;
using EventsFacade.Utilities;
using EventStore.Client;

[assembly: InternalsVisibleTo("EventsFacade.Tests")]

namespace EventsFacade
{
    public class SourceDocumentFacade
    {
        private readonly EventStoreClient _storeClient;

        public SourceDocumentFacade(EventStoreClient storeClient)
        {
            _storeClient = storeClient;
        }

        public async Task<List<DocumentAddedEvent>> GetDocumentsAddedToSourceByAnyUserAsync()
        {
            var literallyEverySingleEventInSystem =  _storeClient.ReadAllAsync(Direction.Forwards, Position.Start);

            var docs = await literallyEverySingleEventInSystem
                .Where(e => e.Event.EventType == nameof(DocumentAddedEvent))
                .Select(ent => Encoding.UTF8.GetString(ent.Event.Data.ToArray()))
                .Select(decoded => JsonSerializer.Deserialize<DocumentAddedEvent>(decoded))
                .ToListAsync();
            return docs;
        }

        public async Task SaveDocumentAddedToSource(AddDocumentToSourceStoreCommand command)
        {
            Debug.WriteLine($"Appending data to {command.UserId.ToUserSourceDocumentsEventsStreamIdentifier()}");
            var @event = new DocumentAddedEvent
            {
                OccurenceDate = DateTime.Now,
                FileName = command.File.FileName,
                FileId = command.Id,
                UserId = command.UserId
            };

            var eventData = new EventData(
                Uuid.NewUuid(),
                nameof(DocumentAddedEvent),
                JsonSerializer.SerializeToUtf8Bytes(@event)
            );

            await _storeClient.AppendToStreamAsync(
                command.UserId.ToUserSourceDocumentsEventsStreamIdentifier(),
                StreamState.Any,
                new[] { eventData }
            );

        }
    }
}