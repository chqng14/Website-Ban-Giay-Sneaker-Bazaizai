namespace App_View.Services;

public interface IUserImageStorage
{
    Task<string> SaveAvatarAsync(IFormFile file, CancellationToken cancellationToken = default);

    bool DeleteAvatar(string? publicPath);
}
