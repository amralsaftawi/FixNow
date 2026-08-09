using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;

public sealed record UploadServiceCategoryIconCommand(
    Guid ServiceCategoryId,
    Stream Content,
    string FileName,
    string ContentType,
    long ContentLength)
    : ICommand<Result<UploadServiceCategoryIconResponse>>
{
    public const long MaxFileSizeBytes = 5L * 1024 * 1024;

    public const string IconFolderPrefix = "service-categories";

    public static readonly IReadOnlySet<string> AllowedExtensions =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".gif",
            ".webp",
            ".svg",
        };
}
