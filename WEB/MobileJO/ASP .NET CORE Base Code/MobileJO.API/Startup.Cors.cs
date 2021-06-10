using Microsoft.Extensions.DependencyInjection;

namespace MobileJO.API
{
    public partial class Startup
    {
        private void ConfigureCors(IServiceCollection services)
        {
            // Default configuration. Allows any origin. Note that this is insecure since
            // it allows any origin to access the API. Please change this according to your 
            // app's needs.
            services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:8080")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
        }        
    }
}
