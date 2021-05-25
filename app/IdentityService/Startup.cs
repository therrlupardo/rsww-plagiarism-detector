using System;
using System.Reflection;
using Common.Extensions;
using Convey;
using Convey.Tracing.Jaeger;
using IdentityService.Models;
using IdentityService.Services;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

using Convey.Tracing.Jaeger;

namespace IdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHealthChecks();
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.AddScoped<IIdentityService, Services.IdentityService>();
            services.AddControllers();
            services.AddRswwSwaggerDocumentation(Assembly.GetExecutingAssembly().GetName().Name);
            services.AddDbContext<UserContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Postgres"));
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });
            services.AddJaeger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API V1"); });
            app.UseRouting();
            app.UseCors("AllowAllHeaders");
            app.UseHealthChecks("/healthcheck");
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}