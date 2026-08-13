using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.UploadServiceRequestImage;

public sealed record UploadServiceRequestImageCommand(
    Guid ServiceRequestId,
    Stream Content,
    string FileName,
    string ContentType,
    long ContentLength)
    : ICommand<Result<UploadServiceRequestImageResponse>>
{
    public const long MaxFileSizeBytes = 5L * 1024 * 1024;

    public const string ProblemImagesFolderPrefix = "FixNow/ServiceRequests";

    public static readonly IReadOnlySet<string> AllowedExtensions =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif",
        };

    public static readonly IReadOnlySet<string> AllowedContentTypes =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/gif",
        };
}
