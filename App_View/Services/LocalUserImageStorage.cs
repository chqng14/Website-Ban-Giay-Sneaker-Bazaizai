using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace App_View.Services;

public sealed class LocalUserImageStorage : IUserImageStorage
{
    private const long MaxFileSize = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    private readonly string _avatarDirectory;

    public LocalUserImageStorage(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var configuredRoot = configuration["Storage:RootPath"] ?? "storage";
        var root = Path.GetFullPath(Path.IsPathRooted(configuredRoot)
            ? configuredRoot
            : Path.Combine(environment.ContentRootPath, configuredRoot));
        _avatarDirectory = Path.Combine(root, "user_img");
        Directory.CreateDirectory(_avatarDirectory);
    }

    public async Task<string> SaveAvatarAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file.Length <= 0 || file.Length > MaxFileSize)
        {
            throw new InvalidDataException("Ảnh đại diện phải nhỏ hơn hoặc bằng 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidDataException("Chỉ chấp nhận ảnh JPG, JPEG, PNG hoặc WEBP.");
        }

        var fileName = $"{Guid.NewGuid():N}.jpg";
        var filePath = Path.Combine(_avatarDirectory, fileName);

        var temporaryPath = Path.Combine(_avatarDirectory, $".{Guid.NewGuid():N}.tmp.jpg");
        try
        {
            await using var input = file.OpenReadStream();
            using var image = await Image.LoadAsync(input, cancellationToken);
            image.Mutate(operation => operation.Resize(new ResizeOptions
            {
                Size = new Size(800, 800),
                Mode = ResizeMode.Max
            }));
            image.Metadata.ExifProfile = null;

            await image.SaveAsJpegAsync(
                temporaryPath,
                new JpegEncoder { Quality = 85 },
                cancellationToken);
            File.Move(temporaryPath, filePath, overwrite: false);
            return $"/user_img/{fileName}";
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

    public bool DeleteAvatar(string? publicPath)
    {
        if (string.IsNullOrWhiteSpace(publicPath)
            || publicPath.Equals("/user_img/default_image.png", StringComparison.OrdinalIgnoreCase)
            || Uri.TryCreate(publicPath, UriKind.Absolute, out _))
        {
            return false;
        }

        var fileName = Path.GetFileName(publicPath);
        var path = Path.GetFullPath(Path.Combine(_avatarDirectory, fileName));
        if (!path.StartsWith(Path.GetFullPath(_avatarDirectory), StringComparison.OrdinalIgnoreCase)
            || !File.Exists(path))
        {
            return false;
        }

        File.Delete(path);
        return true;
    }
}
