namespace AsyncWebPageDownloader.Interfaces
{
    public interface IWebPageDownloadService
    {
        Task DownloadAsync(string url);
    }
}