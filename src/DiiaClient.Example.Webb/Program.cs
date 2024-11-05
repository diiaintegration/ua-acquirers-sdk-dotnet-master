using System.Net;
using DiiaClient.Example.Web;
using DiiaClient.Example.Web.Authorization;
using DiiaClient.Example.Web.Services;
using DiiaClient.CryptoAPI;
using DiiaClient.CryptoService.UAPKI;
using DiiaClient.SDK;
using DiiaClient.SDK.Interfaces;
using static DiiaClient.Helpers.Helper;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllersWithViews();

    ConfigurationManager configuration = builder.Configuration;

    var httpClient = new HttpClient();
    if (!string.IsNullOrEmpty(configuration["DiiaConfig:Proxy:ProxyAddress"]))
    {
        // First create a proxy object
        var proxy = new WebProxy
        {
            Address = new Uri(configuration["DiiaConfig:Proxy:ProxyAddress"]),
            BypassProxyOnLocal = bool.Parse(configuration["DiiaConfig:Proxy:BypassProxyOnLocal"]),
            UseDefaultCredentials = bool.Parse(configuration["DiiaConfig:Proxy:UseDefaultCredentials"])
        };
        // Now create a client handler which uses that proxy
        var httpClientHandler = new HttpClientHandler
        {
            Proxy = proxy,
        };
        // Finally, create the HTTP client object
        httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);
    }

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
    // builder.Services.AddSingleton<ICryptoService>(new CryptoService(configuration[$"{GetPlatform()}:CryptoConfigPath"]));
    builder.Services.AddSingleton<ICryptoService>(new FakeCryptoService());
    builder.Services.AddSingleton<IDiia>(d => new Diia(
        configuration["DiiaConfig:AcquirerToken"],
        configuration["DiiaConfig:AuthAcquirerToken"],
        configuration["DiiaConfig:DiiaHost"],
        httpClient, 
        d.GetService<ICryptoService>()));
}

// Add services to the container.
//builder.Services.AddControllersWithViews();

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // custom basic auth middleware
    app.UseMiddleware<BasicAuthMiddleware>();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

Configuration.Init(app.Configuration);

app.Run();
