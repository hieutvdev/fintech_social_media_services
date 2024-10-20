using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Utilities.FileValidations;

public static class FileValidations
{
  private const long MaxImageSize = 5 * 1024 * 1024;     // 5 MB
  private const long MaxVideoSize = 50 * 1024 * 1024;    // 50 MB
  private const long MaxDocumentSize = 10 * 1024 * 1024; // 10 MB
  private const long MaxAudioSize = 10 * 1024 * 1024;    // 10 MB
  
  public static bool IsImage(string fileName) => ValidationFile(fileName, FileExtension.ImageFileExtensions);
  public static bool IsAudio(string fileName) => ValidationFile(fileName, FileExtension.AudioFileExtensions);
  public static bool IsDocument(string fileName) => ValidationFile(fileName, FileExtension.DocumentFileExtensions);
  public static bool IsVideo(string fileName) => ValidationFile(fileName, FileExtension.VideoFileExtensions);

  private static bool ValidationFile(string fileName, List<string> fileExtensions)
  {
    if (string.IsNullOrWhiteSpace(fileName))
    {
      return false;
    }

    var fileExtension = "." + fileName.Split(".").LastOrDefault();
    return fileExtensions.Contains(fileExtension?.ToLower()!);
  }

  public static string GetFileType(string fileName)
  {
    if (string.IsNullOrWhiteSpace(fileName))
    {
      return "";
    }

    string? fileExtension = fileName.Split(".").LastOrDefault();
    if (fileExtension is null)
    {
      return "";
    }

    if (FileExtension.ImageFileExtensions.Contains($".{fileExtension}"))
    {
      return nameof(FileType.Image);
    }else if (FileExtension.VideoFileExtensions.Contains($".{fileExtension}"))
    {
      return nameof(FileType.Video);
    }else if (FileExtension.AudioFileExtensions.Contains($".{fileExtension}"))
    {
      return nameof(FileType.Audio);
    }else if (FileExtension.DocumentFileExtensions.Contains($".{fileExtension}"))
    {
      return nameof(FileType.Document);
    }

    return "";
  }


  public static bool ValidationFileSize(string fileType, IFormFile? file)
  {
    if (string.IsNullOrWhiteSpace(fileType) || file == null)
    {
      return false;
    }

    if (!Enum.TryParse<FileType>(fileType, true, out var parsedFileType))
    {
      return false;
    }

    switch (parsedFileType)
    {
      case FileType.Image:
        return file.Length <= MaxImageSize;

      case FileType.Video:
        return file.Length <= MaxVideoSize;

      case FileType.Document:
        return file.Length <= MaxDocumentSize;

      case FileType.Audio:
        return file.Length <= MaxAudioSize;

      default:
        return false;
    }
  }

  public static bool ValidationFileStreamSize(string fileType, Stream? fileStream)
  {
    if (string.IsNullOrWhiteSpace(fileType) || fileStream == null)
    {
      return false;
    }

    if (!Enum.TryParse<FileType>(fileType, true, out var parsedFileType))
    {
      return false;
    }

    long fileSize = fileStream.Length;

    switch (parsedFileType)
    {
      case FileType.Image:
        return fileSize <= MaxImageSize;

      case FileType.Video:
        return fileSize <= MaxVideoSize;

      case FileType.Document:
        return fileSize <= MaxDocumentSize;

      case FileType.Audio:
        return fileSize <= MaxAudioSize;

      default:
        return false;
    }
  }

}