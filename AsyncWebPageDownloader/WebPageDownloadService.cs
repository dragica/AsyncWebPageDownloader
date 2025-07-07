using AsyncWebPageDownloader.Interfaces;
using Microsoft.Extensions.Logging;

namespace AsyncWebPageDownloader
{
    public class WebPageDownloadService : IWebPageDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IWebPageDownloadService> _logger;
        private readonly IDownloadConfiguration _configuration;
        private readonly IFileStorageService _streamService;

        public WebPageDownloadService(
            HttpClient httpClient,
            IFileStorageService fileStreamService,
            IDownloadConfiguration configuration,
            ILogger<IWebPageDownloadService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _streamService = fileStreamService;
        }

        public async Task DownloadAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to download {url}. Status code: {response.StatusCode}");
                return;
            }

            var mediaType = response.Content.Headers.ContentType?.MediaType;
            if (mediaType != "text/html")
            {
                _logger.LogWarning($"Unexpected content type: {mediaType} for url {url}.");
                return;
            }

            await using var contentStream = await response.Content.ReadAsStreamAsync();

            var fileName = GenerateFileNameFromUrl(url);

            _logger.LogInformation($"Saving web page {url} to file {_configuration.OutputFolderPath}\\{fileName}.");
            await SaveWebPageAsync(fileName, contentStream);
        }

        private static string GenerateFileNameFromUrl(string url)
        {
            return $"{new Uri(url).Host}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff")}_{Guid.NewGuid().ToString("N")[..6]}.html";
        }

        private async Task SaveWebPageAsync(string webPageFileName, Stream contentStream)
        {
            var outputFilePath = Path.Combine(_configuration.OutputFolderPath, webPageFileName);
            await _streamService.WriteToFileAsync(outputFilePath, contentStream);
        }
    }
}
