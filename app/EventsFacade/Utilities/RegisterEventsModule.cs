using EventsFacade.Services;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace EventsFacade.Utilities
{
    public static class RegisterEventsModule
    {
        public static EventStoreClient GetClient(string connectionString) =>
            new(EventStoreClientSettings.Create(connectionString));

        public static IServiceCollection RegisterEvents(this IServiceCollection services, string connectionString)
        {
            services.AddEventStoreClient(connectionString);

            services.AddTransient<IDocumentAnalysisService, DocumentAnalysisService>();
            services.AddTransient<ISourceDocumentsService, SourceDocumentsService>();

            services.AddTransient<SourceDocumentFacade>();
            services.AddTransient<AnalysisFacade>();

            return services;
        }
    }
}