using System.Reflection;
using Common.Extensions;
using EventsFacade.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OperationContracts;
using QueryService.Services;
using QueryService.Services.Implementations;

namespace QueryService
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
            services.AddControllers();

            services.AddHealthChecks();
            services.AddRswwApiGatewayAuthentication(Configuration);
            services.AddRswwSwaggerGen();
            services.AddRswwSwaggerDocumentation(Assembly.GetExecutingAssembly().GetName().Name);

            services.RegisterEvents(Configuration.GetConnectionString("EventStore"));

            services.AddTransient<IAnalysisService, AnalysisService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<IReportService<AnalysisFile>, AnalysisReportService>();
            services.AddTransient<IDocumentsToAnalysisService, DocumentsToAnalysisService>();
            
            services.AddJaeger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Query API V1"); });
            app.UseRouting();
            app.UseHealthChecks("/healthcheck");
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}