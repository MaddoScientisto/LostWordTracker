using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using LostWordTracker.Core.Services;
using LostWordTracker.Core.Services.Impl;
using LostWordTracker.Services;
using LostWordTracker.Services.Impl;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped(sp =>
     new HttpClient
     {
         BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
     });

            builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAutoMapper(typeof(AutomapperProfile));

            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IGenericLocalStorageService, LocalStorageService>();

            builder.Services.AddScoped<IDatabaseService, HttpDatabaseService>();

            builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

            builder.Services.AddScoped<IDrawingService, DrawingService>();

            await builder.Build().RunAsync();
        }
    }
}
