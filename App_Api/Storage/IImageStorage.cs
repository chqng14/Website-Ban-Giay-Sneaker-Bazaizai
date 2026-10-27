namespace App_Api.Storage;

public interface IImageStorage
{
    Task<string> SaveAsync(
        IFormFile file,
        string folder,
        bool preserveFileName = false,
        CancellationToken cancellationToken = default);

    bool Delete(string folder, string fileName);

    string GetDirectory(string folder);
}
