namespace DiiaClient.CryptoAPI
{
    public interface ICryptoService
    {
        byte[] Decrypt(string data);
        string CalcHash(string data);
    }
}