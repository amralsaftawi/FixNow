using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;

public sealed record RegisterTechnicianCommand(
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey,
    IReadOnlyCollection<Guid> ServiceCategoryIds)
    : ICommand<Result<RegisterTechnicianResponse>>
{
    public const string NationalIdFolderPrefix = "national-ids";
}
