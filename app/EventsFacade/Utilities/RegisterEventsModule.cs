using EventsFacade.Services;
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
            services.AddTransient<DocumentAnalysisService>();
            services.AddTransient<SourceDocumentsService>();
            services.AddTransient<DocumentAnalysisService>();

            return services;
        }
    }
}