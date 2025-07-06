using AsyncWebPageDownloader.Interfaces;

namespace AsyncWebPageDownloader
{
    public class DownloadConfiguration : IDownloadConfiguration
    {
        private const string DefaultInputFolderName = "Content";
        private const string DefaultInputFileName = "urls.txt";
        private const string DefaultOutputFolderName = "Downloads";
        public string UrlsFilePath => Path.Combine(AppContext.BaseDirectory, DefaultInputFolderName, DefaultInputFileName);
        public string OutputFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), DefaultOutputFolderName);
        public int MaxConcurrentDownloads => 5;
    }
}
