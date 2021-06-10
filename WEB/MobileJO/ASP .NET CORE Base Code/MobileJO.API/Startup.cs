using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileJO.API.Hubs;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MobileJO.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureDependencies(services);       // Configuration for dependency injections     
            this.ConfigureDatabase(services);           // Configuration for database connection
            this.ConfigureMapper(services);             // Configuration for entity model and view model mapping (Automapper for API)            
            this.ConfigureCors(services);               // Configuration for CORS        
            this.ConfigureSwaggerGen(services);         // Configuration for Swagger                  
            this.ConfigureAuth(services);               // Configuration for authentication logic
            this.ConfigureMVC(services);                // Configuration for MVC  

            //Add signalR services
            services.AddSignalR(hubOptions =>
            {
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBackgroundJobClient backgroundJobs, UserManager<IdentityUser> userManager, IUserRepository _userRepository, IForgotPasswordRepository _forgotPasswordRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");     // Enables site to redirect to page when an exception occurs
                app.UseHsts();                              // Enables the Strict-Transport-Security header.
            }

            new DBInitializer(_userRepository).SeedIdentityUsers(userManager);   // Creates an initial user upon startup

            app.UseStaticFiles();           // Enables the use of static files
            app.UseHttpsRedirection();      // Enables redirection of HTTP to HTTPS requests.
            app.UseCors("MyCorsPolicy");    // Enables CORS
            app.UseAuthentication();        // Enables the ConfigureAuth service.
            app.UseSignalR(route =>
            {
                route.MapHub<VideoHub>("/video-hub");
                route.MapHub<ImageHub>("/image-hub");
            });


            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate("deleteForgotPassword", () => _forgotPasswordRepository.deleteForgotPasswordBackground(), Cron.Minutely);

            this.ConfigureRoutes(app);      // Configuration for API controller routing
            this.ConfigureSwagger(app);     // Configuration fow Swagger
            this.ConfigureAuth(app);        // Configuration for Token Authentication
        }
    }
}
