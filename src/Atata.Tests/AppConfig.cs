namespace Atata.Tests
{
    public static class AppConfig
    {
        public static string BaseUrl =>
#if NETCOREAPP2_0
            "http://localhost:50549";
#else
            System.Configuration.ConfigurationManager.AppSettings["TestAppUrl"];
#endif
    }
}
