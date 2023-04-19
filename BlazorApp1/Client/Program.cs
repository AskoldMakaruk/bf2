using System.Net;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1.Client;
using Grpc.Core;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
    DefaultRequestVersion = HttpVersion.Version20,
    DefaultRequestHeaders = { }
});

// builder.Services.addgr<Greeter.GreeterClient>(o => {
//     o.Address = new Uri("https://localhost:5001");
// });
builder.Services.AddGrpcClient<ProductionStats.ProductionStatsClient>(o =>
    {
        o.Address = new Uri("https://localhost:7097");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        return handler;
    });

await builder.Build().RunAsync();