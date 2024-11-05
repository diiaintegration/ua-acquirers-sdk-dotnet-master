using DiiaClient.Example.Web.Authorization;
using DiiaClient.Example.Web.Models;
using DiiaClient.SDK.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.DrawingCore.Imaging;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using DiiaClient.SDK.Models.Local;
using DiiaClient.SDK.Models.Remote;
using ZXing;
using ZXing.QrCode;

namespace DiiaClient.Example.Web.Controllers
{
    [Authorize]
    public class SharingController : Controller
    {
        private static UTF8Encoding utf8Encoding = new UTF8Encoding(false);

        private readonly IDiia diia;

        public SharingController(IDiia diia)
        {
            this.diia = diia;
        }

        [HttpGet]
        public IActionResult RequestDocumentByBarcodePage()
        {
            try
            {
                var model = new BarcodeModel();
                return View("RequestByBarcode", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestDocumentByBarcode(BarcodeModel barcodeModel)
        {
            try
            {
                var branches = await diia.GetBranches(0L, 1L);
                var branchId = branches.Branches.FirstOrDefault().Id;
                var requestId = Guid.NewGuid().ToString();
                var model = new IdModel();
                model.RequestId = requestId;
                var isDocumentValid = await diia.RequestDocumentByBarCode(branchId, barcodeModel.Barcode, requestId);
                return View(isDocumentValid ? "RequestSuccess" : "RequestFailed", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RequestDocumentByDeepLink()
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();
                var branches = await diia.GetBranches(0L, 1L);
                var branchId = branches.Branches.FirstOrDefault().Id;
                var offers = await diia.GetOffers(branchId, 0L, 1L);
                var offerId = offers.Offers.FirstOrDefault().Id;
                var deepLink = await diia.GetDeepLink(branchId, offerId, requestId);
                var qrCodeImageBase64 = GenerateQR(deepLink);
                var model = new DeepLinkModel()
                {
                    RequestId = requestId,
                    Deeplink = deepLink,
                    QrCodeImageBase64 = qrCodeImageBase64
                };

                return View("RequestByDeepLink", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult FilesSigningPage()
        {
            try
            {
                var model = new FilesSigningModel();
                return View("FilesSigning", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestFilesSigningDeepLink(FilesSigningModel filesSigningModel)
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();
                var branchId = filesSigningModel.BranchId;
                var offerId = filesSigningModel.OfferId;
                var files = new List<SDK.Models.Remote.File>();
                foreach (var file in filesSigningModel.Files)
                {
                    files.Add(new SDK.Models.Remote.File()
                    {
                        FileName = file.FileName,
                        Data = await ConvertFileToByteArray(file),
                    });
                }
                var deepLink = await diia.GetSignDeepLink(branchId, offerId, requestId, files);
                var qrCodeImageBase64 = GenerateQR(deepLink);
                var model = new DeepLinkModel()
                {
                    RequestId = requestId,
                    BranchId = branchId,
                    OfferId = offerId,
                    Deeplink = deepLink,
                    QrCodeImageBase64 = qrCodeImageBase64
                };

                return View("SignDocumentsByDeepLink", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult AuthPage()
        {
            try
            {
                var model = new AuthModel();
                return View("Auth", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestAuthDeepLink(AuthModel authModel)
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();
                var branchId = authModel.BranchId;
                var offerId = authModel.OfferId;
                //var returnLink = authModel.ReturnLink;
                var deepLink = await diia.GetAuthDeepLink(branchId, offerId, requestId, null);
                var qrCodeImageBase64 = GenerateQR(deepLink.DeepLink);
                var model = new DeepLinkModel()
                {
                    RequestId = deepLink.RequestIdHash,
                    BranchId = branchId,
                    OfferId = offerId,
                    Deeplink = deepLink.DeepLink,
                    QrCodeImageBase64 = qrCodeImageBase64
                };

                return View("AuthByDeepLink", model);
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult ShowSignDocument(string requestId)
        {
            try
            {
                var filePath = Path.Combine(Configuration.DocumentsBaseDir, requestId);

                // scan package folder to find PDF
                if (!System.IO.Directory.Exists(filePath))
                {
                    var model = new ErrorViewModel();
                    model.ErrorMessage = $"Can't find path by requestId {requestId}";
                    return View("Error", model);
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var pdfFile in Directory.GetFiles(filePath, "*.p7s", SearchOption.TopDirectoryOnly))
                        {
                            string fileName = Path.GetFileName(pdfFile);
                            var file = archive.CreateEntry(fileName);
                            using (var entryStream = file.Open())
                            using (var b = new BinaryWriter(entryStream))
                            {
                                b.Write(System.IO.File.ReadAllBytes(Path.Combine(filePath, fileName)));
                            }
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", $"{requestId}.zip");
                }
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message + e.InnerException;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult ShowDocument(string requestId)
        {
            try
            {
                var filePath = Path.Combine(Configuration.DocumentsBaseDir, requestId);

                // scan package folder to find PDF
                if (!System.IO.Directory.Exists(filePath))
                {
                    var model = new ErrorViewModel();
                    model.ErrorMessage = $"Can't find path by requestId {requestId}";
                    return View("Error", model);
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        var jsonFile = archive.CreateEntry("metadata.json");
                        using (var entryStream = jsonFile.Open())
                        using (var streamWriter = new BinaryWriter(entryStream))
                        {
                            streamWriter.Write(System.IO.File.ReadAllBytes(Path.Combine(filePath, "metadata.json")));
                        }

                        foreach (var pdfFile in Directory.GetFiles(filePath, "*.pdf.p7s.p7e", SearchOption.TopDirectoryOnly))
                        {
                            string fileName = Path.GetFileName(pdfFile).Split('.')[0] + ".pdf";
                            var file = archive.CreateEntry(fileName);
                            using (var entryStream = file.Open())
                            using (var b = new BinaryWriter(entryStream))
                            {
                                b.Write(System.IO.File.ReadAllBytes(Path.Combine(filePath, fileName)));
                            }
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", $"{requestId}.zip");
                }
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message + e.InnerException;
                return View("Error", model);
            }
        }

        [HttpGet]
        public IActionResult DecodeAndShowDocument(string requestId)
        {
            try
            {
                var filePath = Path.Combine(Configuration.DocumentsBaseDir, requestId);

                // scan package folder to find PDF
                if (Directory.GetFiles(filePath, "*.pdf.p7s.p7e", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    var model = new ErrorViewModel();
                    model.ErrorMessage = string.Format("Can't find pdf by requestId {0}", requestId);
                    return View("Error", model);
                }

                // prepared to decode
                var headers = new Dictionary<string, string>()
                {
                    {"X-Document-Request-Trace-Id", requestId}
                };
                List<EncodedFile> encodedFiles = new List<EncodedFile>();
                foreach (string pdfFile in Directory.GetFiles(filePath, "*.pdf.p7s.p7e", SearchOption.TopDirectoryOnly))
                {
                    encodedFiles.Add(new EncodedFile()
                    {
                        Name = Path.GetFileName(pdfFile).Split('.')[0] + ".pdf",
                        Data = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Path.Combine(filePath, pdfFile)))
                    });
                }
                var encodeData = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Path.Combine(filePath, "metadata.json.p7s.p7e")));
                // decode all documents
                DocumentPackage decodedDocumentPackage = diia.DecodeDocumentPackage(headers, encodedFiles, encodeData);

                // return it
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        var file1 = archive.CreateEntry("metadata.json");
                        using (var streamWriter = new StreamWriter(file1.Open()))
                        {
                            streamWriter.Write(JsonSerializer.Serialize(decodedDocumentPackage.Data));
                        }

                        foreach (var decodedFile in decodedDocumentPackage.DecodedFiles)
                        {
                            var file = archive.CreateEntry(decodedFile.FileName);
                            using (var streamWriter = new BinaryWriter(file.Open()))
                            {
                                streamWriter.Write(decodedFile.Data);
                            }
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", $"{requestId}.zip");
                }
            }
            catch (Exception e)
            {
                var model = new ErrorViewModel();
                model.ErrorMessage = e.Message + e.InnerException;
                return View("Error", model);
            }
        }


        private string GenerateQR(string url)
        {
            var imgBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";
            try
            {
                QRCodeWriter qrCodeWriter = new QRCodeWriter();
                var bitMatrix = qrCodeWriter.encode(url, BarcodeFormat.QR_CODE, 300, 300);
                var writer = new ZXing.ZKWeb.BarcodeWriter();
                var qrImage = writer.Write(bitMatrix);
                MemoryStream ms = new MemoryStream();
                qrImage.Save(ms, ImageFormat.Png);
                imgBase64 = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e) {
                
            }
            return "data:image/png;base64, " + imgBase64;
        }

        private async Task<byte[]> ConvertFileToByteArray(IFormFile encodeData)
        {
            try
            {
                await using (var ms = new MemoryStream())
                {
                    await encodeData.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
    }
