using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddSwaggerGen();
            services.AddDbContext<UserContext>(options =>
            {
                options.UseNpgsql("host=postgres;port=5432;database=postgres;username=postgres;password=postgres;");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API V1"); });

            app.UseRouting();

            app.UseHealthChecks("/healthcheck");

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}