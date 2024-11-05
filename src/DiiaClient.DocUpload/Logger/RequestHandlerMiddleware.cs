using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DiiaClient.DocUpload.Logger
{
    public sealed class RequestHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestHandlerMiddleware(ILogger<RequestHandlerMiddleware> logger, RequestDelegate next)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var content = JsonSerializer.Serialize(context.Request.Headers, options);

            logger.LogDebug($"Header: {content}");

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            logger.LogDebug($"Body: {body}");
            context.Request.Body.Position = 0;

            logger.LogDebug($"Host: {context.Request.Host.Host}");
            logger.LogDebug($"Client IP: {context.Connection.RemoteIpAddress}");
            await next(context);
        }

    }
}
