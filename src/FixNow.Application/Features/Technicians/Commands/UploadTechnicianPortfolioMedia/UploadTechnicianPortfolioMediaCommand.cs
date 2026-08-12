using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;

public sealed record UploadTechnicianPortfolioMediaCommand(
    Stream Content,
    string FileName,
    string ContentType,
    long ContentLength)
    : ICommand<Result<UploadTechnicianPortfolioMediaResponse>>
{
    public const long MaxFileSizeBytes = 5L * 1024 * 1024;

    public const string PortfolioMediaFolderPrefix = "portfolio-media";

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
