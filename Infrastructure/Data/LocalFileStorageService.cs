using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _http;

    public LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor http)
        => (_env, _http) = (env, http);

    public async Task<string> UploadAsync(
        byte[] data, string fileName, string contentType, CancellationToken ct = default)
    {
        // ensure a web-root even if wwwroot folder is missing
        var webRoot = _env.WebRootPath
                  ?? Path.Combine(_env.ContentRootPath, "wwwroot");

        var folder = Path.Combine(webRoot, "images", "products");
        Directory.CreateDirectory(folder);

        var safeName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        await File.WriteAllBytesAsync(Path.Combine(folder, safeName), data, ct);

        var req = _http.HttpContext!.Request;
        var host = $"{req.Scheme}://{req.Host}";
        return $"{host}/images/products/{safeName}";
    }
}
