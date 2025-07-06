namespace AsyncWebPageDownloader.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<string>> ReadFromFileAsync(string filePath);
        Task WriteToFileAsync(string filePath, Stream inputStream);
    }
}