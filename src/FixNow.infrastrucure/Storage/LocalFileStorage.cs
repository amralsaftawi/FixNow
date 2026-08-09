using FixNow.Application.Common.Interfaces.Storage;

using Microsoft.AspNetCore.Hosting;

namespace FixNow.Infrastructure.Storage;

public sealed class LocalFileStorage : IFileStorage
{
    private const string UploadsDirectoryName = "uploads";

    private readonly string _rootPath;

    public LocalFileStorage(IWebHostEnvironment environment)
        : this(Path.Combine(environment.ContentRootPath, UploadsDirectoryName))
    {
    }

    public LocalFileStorage(string rootPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);

        _rootPath = Path.GetFullPath(rootPath);
    }

    public async Task<Result<string>> StoreAsync(
        string key,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var filePath = ResolveFilePath(key);

        if (filePath is null)
        {
            return StorageErrors.InvalidKey;
        }

        try
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(filePath)!);

            await using var output = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await content.CopyToAsync(
                output,
                cancellationToken);

            return key;
        }
        catch (Exception)
        {
            return StorageErrors.StoreFailed;
        }
    }

    public async Task<Result<Success>> DeleteAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var filePath = ResolveFilePath(key);

        if (filePath is null || !File.Exists(filePath))
        {
            return Result.Success;
        }

        try
        {
            File.Delete(filePath);

            return Result.Success;
        }
        catch (Exception)
        {
            return StorageErrors.DeleteFailed;
        }
    }

    private string? ResolveFilePath(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var fullPath = Path.GetFullPath(
            Path.Combine(_rootPath, key));

        var rootWithSeparator = _rootPath.EndsWith(
            Path.DirectorySeparatorChar)
                ? _rootPath
                : _rootPath + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(
            rootWithSeparator,
            StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return fullPath;
    }
}
