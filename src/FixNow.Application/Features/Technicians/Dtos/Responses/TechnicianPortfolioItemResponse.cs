namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianPortfolioItemResponse(
    Guid PortfolioItemId,
    Guid TechnicianProfileId,
    string Title,
    string? Description,
    IReadOnlyCollection<TechnicianPortfolioMediaResponse> Media);
