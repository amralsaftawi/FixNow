namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianPortfolioMediaResponse(
    Guid PortfolioMediaId,
    string MediaKey,
    int DisplayOrder);
