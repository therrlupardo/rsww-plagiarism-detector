using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading;
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
        public DocumentAnalysisService(EventStoreClient storeClient) : base(storeClient)
        {
        }

        public async Task SaveAnalysisStatusChanged(DocumentAnalysisStatusChangedEvent @event, Guid userId) =>
            await SaveEvent(@event, userId.ToUserAnalysesStreamName());


        private static readonly ConcurrentDictionary<Guid, List<DocumentAnalysisStatusChangedEvent>> Events = new();
        private static long WasInitializedTimes = 0;
        private async Task HandleNewEvent(ResolvedEvent evnt, Guid userId)
        {
            var toDeserialize = Encoding.UTF8.GetString(evnt.Event.Data.ToArray());
            var deserialized = JsonSerializer.Deserialize<DocumentAnalysisStatusChangedEvent>(toDeserialize);

            Console.WriteLine($"noted Analysis changed event for {deserialized.DocumentName}");

            if (Events.TryGetValue(userId, out var events))
            {
                lock (events)
                {
                    events.Add(deserialized);
                }
                return;
            }

            Events.TryAdd(userId, new() { deserialized });
        }

        public async Task<List<DocumentAnalysisStatusChangedEvent>> GetAnalysesForUser(Guid userId)
        {
            if (Interlocked.Read(ref WasInitializedTimes) > 0)
            {
                Console.WriteLine($"Analysis status changed fetched from cache");
                return Events[userId];
            }

            await ResubscribeAsync(Position.Start);
            Interlocked.Add(ref WasInitializedTimes, 1);

            return Events.TryGetValue(userId, out var events)
                ? events
                : new List<DocumentAnalysisStatusChangedEvent>();
        }

        private async Task ResubscribeAsync(Position checkpoint)
        {
            var filter = new SubscriptionFilterOptions(StreamFilter.Prefix(EventStreamNamesUtils.AnalysisStreamPrefix));
            await _storeClient.SubscribeToAllAsync(
                eventAppeared: async (_, e, _) =>
                {
                    var userGuidString =
                        e.OriginalStreamId.Remove(0, EventStreamNamesUtils.AnalysisStreamPrefix.Length);
                    var userGuid = Guid.Parse(userGuidString);
                    await HandleNewEvent(e, userGuid);
                    if (e.OriginalPosition != null) checkpoint = e.OriginalPosition.Value;
                },
                subscriptionDropped: (async (subscription, reason, arg3) =>
                {
                    Console.WriteLine("Analysis subscription dropped");
                    if (reason != SubscriptionDroppedReason.Disposed)
                    {
                        await ResubscribeAsync(checkpoint);
                    }
                }), filterOptions: filter,
                start: checkpoint);
        }
    }
}