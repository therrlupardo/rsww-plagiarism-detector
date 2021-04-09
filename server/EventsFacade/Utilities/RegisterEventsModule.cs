using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace EventsFacade.Utilities
{
    public static class RegisterEventsModule
    {
        public static IServiceCollection RegisterEvents(this IServiceCollection services, string connectionString)
        {
            services.AddEventStoreClient(connectionString);
            services.AddTransient<SourceDocumentFacade>();

            return services;
        }
    }
}