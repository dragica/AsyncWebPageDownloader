using AsyncWebPageDownloader.Interfaces;
using Microsoft.Extensions.Logging;

namespace AsyncWebPageDownloader
{
    public class FileStorageService : IFileStorageService
    {
        private readonly ILogger<IFileStorageService> _logger;
        public FileStorageService(ILogger<IFileStorageService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ReadFromFileAsync(string filePath)
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);

                return lines
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Trim());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to read file '{filePath}': {ex.Message}");
                return [];
            }
        }
        public async Task WriteToFileAsync(string filePath, Stream inputStream)
        {
            try
            {
                await using var fileStream = File.Create(filePath);
                await inputStream.CopyToAsync(fileStream);
            }
            catch (DirectoryNotFoundException)
            {
                _logger.LogError($"Directory not found: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error writing file: {ex.Message}");
            }
        }
    }
}
