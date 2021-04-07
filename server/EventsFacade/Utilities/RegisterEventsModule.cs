using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace EventsFacade.Utilities
{
    public static class RegisterEventsModule
    {
        private const string EventStoreConnectionString = "esdb://eventstore.db:2113?tls=false";
        private static readonly EventStoreClientSettings Settings = EventStoreClientSettings
            .Create(EventStoreConnectionString);

        public static readonly EventStoreClient ClientForProjectsWithoutDI = new(Settings);

        public static IServiceCollection RegisterEvents(this IServiceCollection services)
        {
            services.AddEventStoreClient(EventStoreConnectionString);
            services.AddTransient<SourceDocumentFacade>();

            return services;
        }
    }
}