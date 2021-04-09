using System.Reflection;
using Common.Extensions;
using EventsFacade;
using EventsFacade.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queries;
using QueryService.Services;

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
            services.AddSingleton<IAnalysisService, AnalysisService>();
            services.AddSingleton<ISourceService, SourceService>();
            services.AddSingleton<IReportService<AnalysisFile>, AnalysisReportService>();
            services.RegisterEvents(Configuration.GetConnectionString("EventStore"));
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