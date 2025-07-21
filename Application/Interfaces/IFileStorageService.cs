namespace Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(
        byte[] data,
        string fileName,
        string contentType,
        CancellationToken ct = default);
}
