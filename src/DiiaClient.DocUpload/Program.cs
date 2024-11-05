using DiiaClient.CryptoAPI;
using DiiaClient.CryptoService.UAPKI;
using DiiaClient.DocUpload.Logger;
using DiiaClient.SDK.Interfaces;
using DiiaClient.SDK;
using Microsoft.AspNetCore.HttpLogging;
using static DiiaClient.Helpers.Helper;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

string platform = GetPlatform();

var logPath = configuration[$"{platform}:LogPath"];
if (!System.IO.Directory.Exists(logPath))
    System.IO.Directory.CreateDirectory(logPath);
builder.Logging.AddFile(Path.Combine(logPath, $"logs{DateTime.Now.ToString("yyyy-MM-dd")}.txt"));
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;//HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | 
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ICryptoService>(new CryptoService(configuration[$"{platform}:CryptoConfigPath"]));
builder.Services.AddSingleton<IDiia>(d => new Diia(configuration["acquirerToken"], configuration["authAcquirerToken"], configuration["diiaHost"],
    new HttpClient(), d.GetService<ICryptoService>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseMiddleware<RequestHandlerMiddleware>();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
