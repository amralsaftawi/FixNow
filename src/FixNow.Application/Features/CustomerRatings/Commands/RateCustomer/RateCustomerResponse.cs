namespace FixNow.Application.Features.CustomerRatings.Commands.RateCustomer;

public sealed record RateCustomerResponse(
    Guid CustomerRatingId,
    int Rating);
