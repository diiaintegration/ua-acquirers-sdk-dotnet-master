using static DiiaClient.Helpers.Helper;

namespace DiiaClient.Example.Web
{
    public static class Configuration
    {
        private static string documentsBaseDir;
        private static string userName;
        private static string password;

        public static string DocumentsBaseDir { get { return documentsBaseDir; } }
        public static string UserName { get { return userName; } }
        public static string Password { get { return password; } }

        public static void Init(IConfiguration config)
        {
            documentsBaseDir = config[$"{GetPlatform()}:DocPath"];
            userName = config["Credentials:UserName"];
            password = config["Credentials:Password"];
        }
    }
}
