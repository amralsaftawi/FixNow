using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FixNow.Application.Common.Interfaces.Storage;

namespace FixNow.Infrastructure.Storage;

public sealed class CloudinaryFileStorage(Cloudinary cloudinary) : IFileStorage
{
    private readonly Cloudinary _cloudinary = cloudinary;

    public async Task<Result<string>> StoreAsync(
        string key,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var publicId = NormalizePublicId(key);

        if (string.IsNullOrWhiteSpace(publicId))
        {
            return StorageErrors.InvalidKey;
        }

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(
                Path.GetFileName(key),
                content),
            PublicId = publicId,
            Overwrite = false,
        };

        try
        {
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null)
            {
                return StorageErrors.StoreFailed;
            }

            return uploadResult.PublicId;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
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
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Success;
        }

        var deletionParams = new DeletionParams(key)
        {
            ResourceType = ResourceType.Image,
        };

        try
        {
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Error is not null)
            {
                return StorageErrors.DeleteFailed;
            }

            return Result.Success;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            return StorageErrors.DeleteFailed;
        }
    }

    private static string? NormalizePublicId(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var normalized = key.Trim()
            .Replace('\\', '/')
            .Trim('/');

        var extension = Path.GetExtension(normalized);

        return extension.Length > 0
            ? normalized[..^extension.Length]
            : normalized;
    }
}
