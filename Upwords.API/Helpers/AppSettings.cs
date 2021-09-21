using Microsoft.Extensions.Configuration;

namespace Upwords.API.Helpers
{
    public class AppSettings
    {
        private static IConfiguration configuration;

        public static void Initialize()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }

        public static IConfiguration Initialize(string environmentName)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            configuration = builder.Build();

            return configuration;
        }

        public static string GetValue(string section)
        {
            if (configuration == null)
            {
                configuration = Initialize("local");
            }

            return configuration.GetSection(section).Get<string>();
        }
    }
}
