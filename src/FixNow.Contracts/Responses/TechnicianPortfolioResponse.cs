namespace FixNow.Contracts.Responses;

public sealed record TechnicianPortfolioResponse(
    IReadOnlyCollection<TechnicianPortfolioItemResponse> Items);
