using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace DiiaClient.DocUpload.Helpers
{
    public static class MutipartMixedHelper
    {
        public static async IAsyncEnumerable<ParsedSection> ParseMultipartMixedRequestAsync(HttpRequest request)
        {
            // Extract, sanitize and validate boundry
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(request.ContentType).Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary) ||
                (boundary.Length > new FormOptions().MultipartBoundaryLengthLimit))
            {
                throw new InvalidDataException("boundry is shot");
            }

            // Create a new reader based on that boundry
            var reader = new MultipartReader(boundary, request.Body);

            // Start reading sections from the MultipartReader until there are no more
            MultipartSection section;
            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                // parse the content type
                var contentType = new ContentType(section.ContentType ?? "text/plain");

                // create a new ParsedSecion and start filling in the details
                var parsedSection = new ParsedSection
                {
                    IsJson = contentType.MediaType.Equals("application/json",
                        StringComparison.OrdinalIgnoreCase),
                    IsXml = contentType.MediaType.Equals("text/xml",
                        StringComparison.OrdinalIgnoreCase),
                    IsText = contentType.MediaType.Equals("text/plain",
                        StringComparison.OrdinalIgnoreCase),
                    IsFile = contentType.MediaType.Equals("application/octet-stream",
                        StringComparison.OrdinalIgnoreCase),
                    Encoding = Encoding.UTF8//GetEncoding(contentType.CharSet)
                };

                // Must be XML or JSON
                if (!parsedSection.IsXml && !parsedSection.IsJson && !parsedSection.IsText)
                {
                    throw new InvalidDataException("only handling json/xml/text");
                }

                // Parse the content disosition
                if (ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition) &&
                        (contentDisposition.DispositionType.Value.Equals("form-data")))
                {
                    // save the name
                    parsedSection.Name = contentDisposition.Name.Value;
                    parsedSection.FileName = contentDisposition.FileName.Value;

                    // Create a new StreamReader using the proper encoding and
                    // leave the underlying stream open
                    using (var streamReader = new StreamReader(
                        section.Body, parsedSection.Encoding, leaveOpen: true))
                    {
                        parsedSection.Data = await streamReader.ReadToEndAsync();
                        yield return parsedSection;
                    }
                }
            }
        }
    }

    public sealed class ParsedSection
    {
        public bool IsJson { get; set; }
        public bool IsXml { get; set; }
        public bool IsText { get; set; }
        public bool IsFile { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Data { get; set; }
        public Encoding Encoding { get; set; }
    }
}
