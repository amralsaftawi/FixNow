namespace FixNow.Contracts.Responses;

public sealed record TechnicianPortfolioItemResponse(
    Guid PortfolioItemId,
    Guid TechnicianProfileId,
    string Title,
    string? Description,
    IReadOnlyCollection<TechnicianPortfolioMediaResponse> Media);
