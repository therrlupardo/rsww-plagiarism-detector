using Microsoft.AspNetCore.Hosting;

namespace Common.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static void UseCommonSentry(this IWebHostBuilder webBuilder)
        {
            webBuilder.UseSentry("https://e90b425aabc54e21be0900d423244186@o707948.ingest.sentry.io/5778611");
        }
    }
}