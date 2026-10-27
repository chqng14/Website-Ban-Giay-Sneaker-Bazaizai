namespace App_Api.Storage;

public sealed class StorageOptions
{
    public const string SectionName = "Storage";

    public string RootPath { get; set; } = "storage";

    public long MaxImageSizeBytes { get; set; } = 5 * 1024 * 1024;
}
