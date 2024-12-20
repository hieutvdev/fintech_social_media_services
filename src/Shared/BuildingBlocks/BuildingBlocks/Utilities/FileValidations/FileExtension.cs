namespace BuildingBlocks.Utilities.FileValidations;

public static class FileExtension
{
    public static readonly List<string> ImageFileExtensions = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".bmp",
        ".tiff",
        ".webp"
    };

    public static readonly List<string> DocumentFileExtensions = new()
    {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".ppt",
        ".pptx",
        ".txt",
        ".rtf"
    };

    public static readonly List<string> AudioFileExtensions = new()
    {
        ".mp3",
        ".wav",
        ".aac",
        ".flac",
        ".ogg",
        ".wma",
        ".m4a"
    };

    public static readonly List<string> VideoFileExtensions = new()
    {
        ".mp4",
        ".avi",
        ".mov",
        ".wmv",
        ".flv",
        ".mkv",
        ".webm"
    };
}