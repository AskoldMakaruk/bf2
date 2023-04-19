using BlazorApp1.Server.Controllers;
using EconomicSimulator.Lib;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.ConfigureKestrel(
//     options =>
//         options.ConfigureEndpointDefaults(listenOptions =>
//         {
//             listenOptions.Protocols = HttpProtocols.Http2;
//         })
// );

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddGrpc(options => { options.EnableDetailedErrors = true; });
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapGrpcReflectionService();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions()
{
    DefaultEnabled = true
});
app.MapGrpcService<ProductionStatsService>().EnableGrpcWeb();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.RunAsync();


StartStatic.Start();