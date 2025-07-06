namespace AsyncWebPageDownloader.Interfaces
{
    public interface IDownloadConfiguration
    {
        string UrlsFilePath { get; }
        string OutputFolderPath { get; }
        int MaxConcurrentDownloads { get; }
    }
}