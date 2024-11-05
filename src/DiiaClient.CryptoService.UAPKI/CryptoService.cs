using System.Runtime.InteropServices;
using DiiaClient.CryptoAPI;
using UapkiWrapperCore;

namespace DiiaClient.CryptoService.UAPKI
{
    public class CryptoService : ICryptoService
    {
        private readonly UapkiWrapper wrapper = new();
        public CryptoService(string configPath)
        {
            string path = Directory.GetParent(typeof(CryptoService).Assembly.Location)!.FullName;
            string pathLib = Path.Combine(path, "libs", "uapki", "libs", GetLib());
            configPath = Path.Combine(path, configPath);
            try
            {
                UapkiWrapper.Result result = wrapper.Init(configPath, pathLib);
                Thread.Sleep(1000);
                if (result == null)
                {
                    throw new Exception($"Init error CryptoService. pathLib: {pathLib}. pathConf: {configPath}.");
                }
                if (result != null && result.ErrorCode != 0)
                {
                    throw new Exception($"Init error CryptoService. pathLib: {pathLib}. pathConf: {configPath}. " +
                        $"ErrorCode: {result.ErrorCode} ErrorMessage: {result.ErrorMessage} Method: {result.Method}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Init error CryptoService. pathLib:{pathLib}. pathConf:{configPath}. Message:{ex}");
            }
        }
        public byte[] Decrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new Exception("No data to decrypt");
            try
            {
                UapkiWrapper.Result retUnwrap = wrapper.Unwrap(data);

                if (retUnwrap.ErrorCode != 0)
                {
                    throw new Exception($"Unwrap error. " +
                        $"ErrorCode: {retUnwrap.ErrorCode} ErrorMessage: {retUnwrap.ErrorMessage} Method: {retUnwrap.Method}");
                }

                return Convert.FromBase64String(retUnwrap.ResultData.Base64);
            }
            catch (Exception e)
            {
                throw new Exception($"Unwrap error. Message: {e}");
            }
        }

        public string CalcHash(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new Exception("No data to calculate hash");
            try
            {
                UapkiWrapper.Result calcHash = wrapper.DigestGost34311(data);

                if (calcHash.ErrorCode != 0)
                {
                    throw new Exception($"CalcHash error. " +
                        $"ErrorCode: {calcHash.ErrorCode} ErrorMessage: {calcHash.ErrorMessage} Method: {calcHash.Method}");
                }

                return calcHash.ResultData.Base64;
            }
            catch (Exception e)
            {
                throw new Exception($"CalcHash error. Message: {e}");
            }
        }
        private string GetLib()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "windows_x86-64\\";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "linux_x86-64/";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "mac_x86-64/";
            }
            else
            {
                throw new PlatformNotSupportedException("Not supported OS");
            }
        }
    }
}