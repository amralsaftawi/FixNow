namespace FixNow.Contracts.Responses;

public sealed record TechnicianPortfolioMediaResponse(
    Guid PortfolioMediaId,
    string MediaKey,
    int DisplayOrder);
