using SportsOddsAnalyzer;
using SportsOddsAnalyzer.Services;
using SportsOddsAnalyzer.Interfaces;
using SportsOddsAnalyzer.Providers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ITheOddsProvider, TheOddsProvider>();
builder.Services.AddScoped<ITheOddsService, TheOddsService>();

await builder.Build().RunAsync();