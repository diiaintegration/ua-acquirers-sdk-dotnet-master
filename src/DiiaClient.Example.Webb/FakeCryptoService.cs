using DiiaClient.CryptoAPI;

namespace DiiaClient.Example.Web;

public class FakeCryptoService : ICryptoService
{
    public byte[] Decrypt(string data)
    {
        return Array.Empty<byte>();
    }

    public string CalcHash(string data)
    {
        return string.Empty;
    }
}