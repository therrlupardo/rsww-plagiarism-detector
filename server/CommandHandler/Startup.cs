using CommandHandler.Extensions;
using CommandHandler.Handlers;
using Commands;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommandHandler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMqConnection(Configuration.GetSection("rabbitmq"));
            services.AddTransient<IHandler<AddDocumentToSourceStoreCommand>, AddDocumentToSourceStoreCommandHandler>();
            services.AddTransient<IHandler<VerifyDocumentCommand>, VerifyDocumentCommandHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.AddHandler<AddDocumentToSourceStoreCommand>();
            app.AddHandler<VerifyDocumentCommand>();
        }
    }
}