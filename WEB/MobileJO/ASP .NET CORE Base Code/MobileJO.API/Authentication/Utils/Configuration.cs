using Microsoft.Extensions.Configuration;
using MobileJO.Data;
using System.IO;

namespace MobileJO.API.Utilities
{
    public class Configuration
    {
        public static IConfigurationRoot Config { get; set; }

        static Configuration()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile(Constants.Common.AppSettingsJson);
            Config = builder.Build();

            Configuration.Config = Config;
        }

        public static string DbConnection => Config[Constants.Common.DefaultConnection];
    }
}

