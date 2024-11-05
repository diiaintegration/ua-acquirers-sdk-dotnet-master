namespace DiiaClient.SDK.Exception
{
    public class DiiaClientException : System.Exception
    {
        public DiiaClientException()
        {
        }

        public DiiaClientException(string? message):base(message)
        {
        }

        public DiiaClientException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
