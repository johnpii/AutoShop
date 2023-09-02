namespace AutoShop.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config;
        public static TimeSpan expireTimeCookie = TimeSpan.FromMinutes(5);
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
    }
}
