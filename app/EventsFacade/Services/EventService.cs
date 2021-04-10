using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventStore.Client;

namespace EventsFacade.Services
{
    public abstract class EventService
    {
        private readonly EventStoreClient _storeClient;

        protected EventService(EventStoreClient storeClient)
        {
            _storeClient = storeClient;
        }

        protected async Task<List<TEvent>> GetAllEventsFromStream<TEvent>(string stream) where TEvent : BaseEvent
        {
             var events = _storeClient.ReadStreamAsync(
                Direction.Forwards, stream, StreamPosition.Start);

            var docs = await events
                .Select(ent => Encoding.UTF8.GetString((byte[]) ent.Event.Data.ToArray()))
                .Select(decoded => JsonSerializer.Deserialize<TEvent>(decoded))
                .ToListAsync();

            return docs;
        }

        public async Task SaveEvent<TEvent>(TEvent @event, string stream) where TEvent : BaseEvent
        {
            Debug.WriteLine($"Appending data to {stream}");

            var eventData = new EventData(Uuid.NewUuid(),
                @event.GetType().Name,
                JsonSerializer.SerializeToUtf8Bytes(@event));

            await _storeClient.AppendToStreamAsync(stream, StreamState.Any, new []{eventData});
        }
    }
}