using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace App_Api.Storage;

public sealed class LocalImageStorage : IImageStorage
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    private readonly string _rootPath;
    private readonly long _maxImageSizeBytes;

    public LocalImageStorage(
        IOptions<StorageOptions> options,
        IWebHostEnvironment environment)
    {
        var configuredRoot = options.Value.RootPath;
        _rootPath = Path.GetFullPath(
            Path.IsPathRooted(configuredRoot)
                ? configuredRoot
                : Path.Combine(environment.ContentRootPath, configuredRoot));
        _maxImageSizeBytes = options.Value.MaxImageSizeBytes;
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> SaveAsync(
        IFormFile file,
        string folder,
        bool preserveFileName = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (file.Length <= 0 || file.Length > _maxImageSizeBytes)
        {
            throw new InvalidDataException(
                $"Ảnh phải có dung lượng từ 1 byte đến {_maxImageSizeBytes / 1024 / 1024} MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidDataException("Chỉ chấp nhận ảnh JPG, JPEG, PNG hoặc WEBP.");
        }

        var directory = GetDirectory(folder);
        Directory.CreateDirectory(directory);

        var fileName = preserveFileName
            ? BuildSafeOriginalFileName(file.FileName)
            : $"{Guid.NewGuid():N}{extension}";
        var outputPath = GetSafeFilePath(directory, fileName);

        if (preserveFileName && File.Exists(outputPath))
        {
            throw new IOException($"Ảnh '{fileName}' đã tồn tại.");
        }

        var temporaryPath = GetSafeFilePath(directory, $".{Guid.NewGuid():N}.tmp{extension}");
        try
        {
            await using var input = file.OpenReadStream();
            using var image = await Image.LoadAsync(input, cancellationToken);
            image.Metadata.ExifProfile = null;
            image.Mutate(operation => operation.Resize(new ResizeOptions
            {
                Size = new Size(1600, 1600),
                Mode = ResizeMode.Max
            }));

            await image.SaveAsync(temporaryPath, cancellationToken);
            File.Move(temporaryPath, outputPath, overwrite: false);
            return fileName;
        }
        catch (UnknownImageFormatException exception)
        {
            throw new InvalidDataException("Ná»™i dung file khÃ´ng pháº£i lÃ  áº£nh há»£p lá»‡.", exception);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    public bool Delete(string folder, string fileName)
    {
        var directory = GetDirectory(folder);
        var safeFileName = Path.GetFileName(fileName);
        var path = GetSafeFilePath(directory, safeFileName);
        if (!File.Exists(path))
        {
            return false;
        }

        File.Delete(path);
        return true;
    }

    public string GetDirectory(string folder)
    {
        var safeFolder = folder.Replace('\\', '/').Trim('/');
        if (safeFolder.Split('/').Any(segment => segment is "." or ".." || string.IsNullOrWhiteSpace(segment)))
        {
            throw new InvalidDataException("Thư mục lưu ảnh không hợp lệ.");
        }

        var directory = Path.GetFullPath(Path.Combine(_rootPath, safeFolder));
        EnsureInsideRoot(directory);
        return directory;
    }

    private static string BuildSafeOriginalFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var invalidCharacters = Path.GetInvalidFileNameChars();
        var safeBaseName = new string(baseName
            .Where(character => !invalidCharacters.Contains(character))
            .ToArray())
            .Trim();

        if (string.IsNullOrWhiteSpace(safeBaseName))
        {
            throw new InvalidDataException("Tên ảnh không hợp lệ.");
        }

        return safeBaseName + extension;
    }

    private string GetSafeFilePath(string directory, string fileName)
    {
        var path = Path.GetFullPath(Path.Combine(directory, fileName));
        EnsureInsideRoot(path);
        return path;
    }

    private void EnsureInsideRoot(string path)
    {
        var relativePath = Path.GetRelativePath(_rootPath, path);
        if (relativePath == ".." || relativePath.StartsWith($"..{Path.DirectorySeparatorChar}"))
        {
            throw new InvalidDataException("Đường dẫn lưu ảnh không hợp lệ.");
        }
    }
}
