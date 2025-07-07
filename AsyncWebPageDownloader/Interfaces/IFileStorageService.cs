namespace AsyncWebPageDownloader.Interfaces
{
    public interface IFileStorageService
    {
        Task<IEnumerable<string>> ReadFromFileAsync(string filePath);
        Task WriteToFileAsync(string filePath, Stream inputStream);
    }
}