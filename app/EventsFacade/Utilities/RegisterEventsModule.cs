using EventsFacade.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventsFacade.Utilities
{
    public static class RegisterEventsModule
    {
        public static IServiceCollection RegisterEvents(this IServiceCollection services, string connectionString)
        {
            services.AddEventStoreClient(connectionString);

            services.AddTransient<IDocumentAnalysisService, DocumentAnalysisService>();
            services.AddTransient<ISourceDocumentsService, SourceDocumentsService>();
            services.AddTransient<IDocumentsToAnalysisService, DocumentsToAnalysisService>();

            services.AddTransient<SourceDocumentFacade>();
            services.AddTransient<AnalysisFacade>();
            services.AddTransient<DocumentsToAnalysisFacade>();

            return services;
        }
    }
}