using System;
using System.Threading;
using CommandHandler.Handlers;
using Commands;
using Microsoft.AspNetCore.Builder;
using RawRabbit;

namespace CommandHandler.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IApplicationBuilder AddRabbitMqCommandHandler<T>(this IApplicationBuilder app, IBusClient client)
            where T : BaseCommand
        {
            if (!(app.ApplicationServices.GetService(typeof(IHandler<T>)) is IHandler<T> handler))
                throw new NullReferenceException();

            client.SubscribeAsync<T>(async (msg, context) =>
            {
                await handler.HandleAsync(msg, CancellationToken.None);
            });

            return app;
        }

        public static IApplicationBuilder AddRabbitMqCommandHandler<T>(this IApplicationBuilder app)
            where T : BaseCommand
        {
            if (!(app.ApplicationServices.GetService(typeof(IBusClient)) is IBusClient busClient))
                throw new NullReferenceException();

            return AddRabbitMqCommandHandler<T>(app, busClient);
        }
    }
}