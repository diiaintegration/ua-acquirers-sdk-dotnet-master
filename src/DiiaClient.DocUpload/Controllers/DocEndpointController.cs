using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using DiiaClient.DocUpload.Helpers;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;
using Microsoft.AspNetCore.Mvc;
using static DiiaClient.Helpers.Helper;

namespace DiiaClient.DocUpload.Controllers
{
    [ApiController]
    [Route("doc")]
    public class DocEndpointController : ControllerBase
    {
        private readonly string baseDocDir;
        private readonly string HeaderNameRequestId = "X-Document-Request-Trace-Id";
        private readonly string HeaderNameDiiaIdAction = "X-Diia-Id-Action";
        private static UTF8Encoding utf8Encoding = new UTF8Encoding(false);

        private readonly IDiia _diia;

        private readonly ILogger<DocEndpointController> _logger;
        private readonly IConfiguration _config;

        public DocEndpointController(ILogger<DocEndpointController> logger, IConfiguration config, IDiia diia)
        {
            _logger = logger;
            _config = config;
            baseDocDir = _config[$"{GetPlatform()}:DocPath"];
            _diia = diia;
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "Endpoint OK!";
        }
        /// <summary>
        /// For Content-Type multipart/form-data
        /// </summary>
        [HttpPost]
        [Route("upload")]
        public async Task<string> Upload(IFormCollection collection)
        {

            #region Validation

            Dictionary<string, string> headers = Request.Headers.ToDictionary(a => a.Key, a => string.Join(";", a.Value));
            string requestId = headers.FirstOrDefault(x => x.Key.Equals(HeaderNameRequestId, StringComparison.OrdinalIgnoreCase)).Value;
            if (string.IsNullOrEmpty(requestId))
            {
                _logger.LogError($"Could not find {HeaderNameRequestId}");
                return JsonSerializer.Serialize(new
                {
                    success = false
                });
            }
            
            #endregion

            try
            {
                _logger.LogInformation($"RequestId: {requestId}. Upload start...");

                string diiaIdAction = headers.FirstOrDefault(x => x.Key.Equals(HeaderNameDiiaIdAction, StringComparison.OrdinalIgnoreCase)).Value;

                #region Auth&Sign
                if (!string.IsNullOrEmpty(diiaIdAction) && (diiaIdAction == "hashedFilesSigning" || diiaIdAction == "auth"))
                {
                    await DiiaAction(headers, requestId, diiaIdAction);
                }
                #endregion

                #region Documents
                else
                {
                    #region Save encoded data

                    // create directory for the documents pack
                    var path = Path.Combine(baseDocDir, requestId);

                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    List<IFormFile> documents = collection.Files.Where(x => !string.IsNullOrEmpty(x.FileName)).ToList();
                    List<EncodedFile> encodedFiles = new List<EncodedFile>();
                    foreach (var document in documents)
                    {
                        _logger.LogInformation($"RequestId: {requestId}. FileName:{document.FileName}" +
                                               $" Name:{document.Name} " +
                                               $" ContentDisposition: {document.ContentDisposition} " +
                                               $" ContentType: {document.ContentType} " +
                                               $" Length: {document.Length}");

                        await SaveEncodedDocumentToDir(document, path);

                        // map received documents to EncodedFile structure
                        var encFile = new EncodedFile()
                        {
                            Data = await ConvertFileToBase64String(document),
                            Name = document.FileName.Split('.')[0] + ".pdf"
                        };
                        encodedFiles.Add(encFile);
                    }

                    string encodeData = collection["encodeData"].ToString();
                    if (string.IsNullOrEmpty(encodeData))
                        _logger.LogError($"RequestId: {requestId}. Empty encodeData");
                    else
                        await SaveDataToDir(encodeData, Path.Combine(path, "metadata.json.p7s.p7e"));

                    #endregion

                    #region Save decoded data

                    if (_config["DecodeOnEndpoint"].ToLower() == "true")
                    {
                        // decode all documents
                        DocumentPackage decodedDocumentPackage = _diia.DecodeDocumentPackage(headers, encodedFiles, encodeData);

                        // save all decoded PDF documents
                        decodedDocumentPackage.DecodedFiles.ForEach(document => SaveDecodedDocumentToDir(document, path));

                        // save documents metadata
                        SaveDecodedMetadataToDir(decodedDocumentPackage.Data, path);
                    }

                    #endregion
                }
#endregion
                
                _logger.LogInformation($"RequestId: {requestId}. Upload end.");

                return JsonSerializer.Serialize(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"RequestId: {requestId}. Error Upload: {e}");
                return JsonSerializer.Serialize(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// For Content-Type multipart/mixed
        /// </summary>
        [HttpPost]
        [Route("upload")]
        public async Task<string> Upload()
        {

            #region Validation

            Dictionary<string, string> headers = Request.Headers.ToDictionary(a => a.Key, a => string.Join(";", a.Value));
            string requestId = headers.FirstOrDefault(x => x.Key.Equals(HeaderNameRequestId, StringComparison.OrdinalIgnoreCase)).Value;
            if (string.IsNullOrEmpty(requestId))
            {
                _logger.LogError($"Could not find {HeaderNameRequestId}");
                return JsonSerializer.Serialize(new
                {
                    success = false
                });
            }

            #endregion

            try
            {
                _logger.LogInformation($"RequestId: {requestId}. Upload start...");
                string diiaIdAction = headers.FirstOrDefault(x => x.Key.Equals(HeaderNameDiiaIdAction, StringComparison.OrdinalIgnoreCase)).Value;

                #region DecodeSignaturePackage

                if (!string.IsNullOrEmpty(diiaIdAction) && (diiaIdAction == "hashedFilesSigning" || diiaIdAction == "auth"))
                {
                    await DiiaAction(headers, requestId, diiaIdAction);
                }
                else
                    throw new Exception("Not implementeted way");
                #endregion

                _logger.LogInformation($"RequestId: {requestId}. Upload end.");

                return JsonSerializer.Serialize(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"RequestId: {requestId}. Error Upload: {e}");
                return JsonSerializer.Serialize(new
                {
                    success = false
                });
            }
        }

        #region private

        private async Task DiiaAction(Dictionary<string, string> headers, string requestId, string diiaIdAction)
        {
            List<ParsedSection> parsedSections = await MutipartMixedHelper.ParseMultipartMixedRequestAsync(Request).ToListAsync();

            var encodeData = parsedSections.FirstOrDefault(x => x.Name == "encodeData")?.Data;

            if (string.IsNullOrEmpty(encodeData))
                throw new Exception("Empty encodeData");

            _logger.LogDebug($"RequestId: {requestId}. diiaIdAction: hashedFilesSigning. diiaIdAction: {diiaIdAction}. encodeData: {encodeData}");

            var signaturePackage = _diia.DecodeSignaturePackage(headers, encodeData);

            string path;
            if(diiaIdAction == "auth")
                path = Path.Combine(baseDocDir, System.Text.RegularExpressions.Regex.Replace(requestId, @"\W+", "_"));
            else
                path = Path.Combine(baseDocDir, requestId);

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            foreach (var signature in signaturePackage.Signatures)
            {
                await SaveDataToDir(signature.Signature, Path.Combine(path, signature.Filename));
            }
        }

        private async Task<string> ConvertFileToBase64String(IFormFile encodeData)
        {
            try
            {
                await using(var ms = new MemoryStream())
                {
                    await encodeData.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    return utf8Encoding.GetString(fileBytes);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Can't read document data from {encodeData.FileName}: {e}");
                return null;
            }
        }

        private async Task SaveEncodedDocumentToDir(IFormFile document, string path)
        {
            string fileName = Path.Combine(path, document.FileName.Replace(":", "-"));
            try
            {
                await using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await document.CopyToAsync(fileStream);
                }
                _logger.LogInformation($"FileName:{fileName} saved");
            }
            catch (Exception e)
            {
                _logger.LogError($"Can't save encoded document {fileName}: {e}");
            }
        }

        private async Task SaveDataToDir(string data, string pathFileName)
        {
            try
            {
                await System.IO.File.WriteAllBytesAsync(pathFileName, utf8Encoding.GetBytes(data));
                _logger.LogInformation($"FileName:{pathFileName} saved");
            }
            catch (Exception e)
            {
                _logger.LogError($"Can't save encoded document {pathFileName}: {e}");
            }
        }

        private void SaveDecodedDocumentToDir(DecodedFile document, string path)
        {
            string fileName = Path.Combine(path, document.FileName.Replace(":", "-"));
            try
            {
                System.IO.File.WriteAllBytes(fileName, document.Data);
                _logger.LogInformation($"FileName:{fileName} saved");
            }
            catch (Exception e)
            {
                _logger.LogError($"Can't save decoded document {fileName}: {e}");
            }
        }

        private void SaveDecodedMetadataToDir(Metadata metadata, string path)
        {
            string fileName = Path.Combine(path, "metadata.json");
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                };
                string json = JsonSerializer.Serialize(metadata, options);
                System.IO.File.WriteAllBytes(fileName, utf8Encoding.GetBytes(json));
                _logger.LogInformation($"FileName:{fileName} saved");
            }
            catch (IOException e)
            {
                _logger.LogError($"Can't save decoded metadata {fileName}: {e}");
            }
        }

        #endregion

    }
}