# AsyncWebPageDownloader

AsyncWebPageDownloader is a .NET 8 console application for efficiently downloading multiple web pages asynchronously. It reads a list of URLs from a file, downloads each page using configurable concurrency, and saves the HTML content to disk.

## Features

- Asynchronous downloading of web pages using `HttpClient`
- Configurable maximum concurrent downloads
- Reads URLs from a configurable file
- Saves each web page as an HTML file with a unique, timestamped filename
- Logs download progress, errors, and warnings

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installing

Open the project in Visual Studio, open a new terminal and type the following commands:

```
dotnet restore
dotnet run --project ./AsyncWebPageDownloader
```
### Running

Observe the logs in the Terminal window and navigate to Downloads folder to access the downloaded files.
