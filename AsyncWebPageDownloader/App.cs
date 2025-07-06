using AsyncWebPageDownloader.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AsyncWebPageDownloader
{
    public class App
    {
        private readonly IFileService _fileService;
        private readonly IWebPageDownloadService _downloadService;
        private readonly IDownloadConfiguration _configuration;
        private readonly ILogger<App> _logger;

        public App(
            IWebPageDownloadService downloadService,
            IFileService fileService,
            IDownloadConfiguration configuration,
            ILogger<App> logger)
        {
            _downloadService = downloadService;
            _fileService = fileService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Run()
        {
            await DownloadWebPagesAsync(_logger);
        }

        private async Task DownloadWebPagesAsync(ILogger<App> logger)
        {
            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(_configuration.MaxConcurrentDownloads);
            var urls = await _fileService.ReadFromFileAsync(_configuration.UrlsFilePath);

            var stopwatch = Stopwatch.StartNew();

            foreach (var url in urls)
            {
                await semaphore.WaitAsync();

                var task = Task.Run(async () =>
                {
                    try
                    {
                        await _downloadService.DownloadAsync(url);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();
            _logger.LogInformation($"Downloads completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
