using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using HopflyApi.Models;
using HopflyApi.Services;

namespace HopflyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });

                options.AddPolicy("AnotherPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                        
                });
            });
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
            services.AddDbContext<HopflyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ILogementService, LogementService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ITicketService, TicketService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(); 

            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}