using AsyncWebPageDownloader;
using AsyncWebPageDownloader.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient<IWebPageDownloadService, WebPageDownloadService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));       
        });
        services.AddSingleton<IDownloadConfiguration, DownloadConfiguration>();
        services.AddSingleton<IWebPageDownloadService, WebPageDownloadService>();
        services.AddSingleton<IFileService, FileService>();

        services.AddLogging(configure => configure.AddDebug().AddConsole());
        services.AddSingleton<App>();
    })
    .Build();

var app = host.Services.GetRequiredService<App>();
await app.Run();