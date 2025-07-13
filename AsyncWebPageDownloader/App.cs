using AsyncWebPageDownloader.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AsyncWebPageDownloader
{
    public class App
    {
        private readonly IFileStorageService _fileService;
        private readonly IWebPageDownloadService _downloadService;
        private readonly IDownloadConfiguration _configuration;
        private readonly ILogger<App> _logger;
        private readonly SemaphoreSlim _semaphore;

        public App(
            IWebPageDownloadService downloadService,
            IFileStorageService fileService,
            IDownloadConfiguration configuration,
            ILogger<App> logger)
        {
            _downloadService = downloadService;
            _fileService = fileService;
            _configuration = configuration;
            _logger = logger;
            _semaphore = new SemaphoreSlim(_configuration.MaxConcurrentDownloads);

        }

        public async Task Run()
        {
            await DownloadWebPagesAsync();
        }

        private async Task DownloadWebPagesAsync()
        {
            var tasks = new List<Task>();

            // For smaller datasets, loading all URLs into memory is acceptable.
            var urls = await _fileService.ReadFromFileAsync(_configuration.UrlsFilePath);

            var stopwatch = Stopwatch.StartNew();

            foreach (var url in urls)
            {
                tasks.Add(DownloadWithSemaphoreAsync(url));

            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();
            _logger.LogInformation($"Downloads completed in {stopwatch.ElapsedMilliseconds} ms.");
            await Task.Delay(500); // give logger time to flush
        }

        async Task DownloadWithSemaphoreAsync(string url)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _downloadService.DownloadAsync(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading URL: {url}");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
