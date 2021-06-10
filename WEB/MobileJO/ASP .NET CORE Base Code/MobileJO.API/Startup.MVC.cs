using Microsoft.Extensions.DependencyInjection;

namespace MobileJO.API
{
    public partial class Startup
    {
        private void ConfigureMVC(IServiceCollection services)
        {
            services.AddMvc().AddWebApiConventions();
        }
    }
}
