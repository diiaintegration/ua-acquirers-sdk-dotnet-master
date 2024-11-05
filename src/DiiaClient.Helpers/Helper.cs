using System.Runtime.InteropServices;

namespace DiiaClient.Helpers
{
    public class Helper
    {
        public static string GetPlatform()
        {
            string platform;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                platform = "Windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                platform = "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                platform = "OSX";
            else throw new PlatformNotSupportedException("Not supported OS");
            return platform;
        }
    }
}