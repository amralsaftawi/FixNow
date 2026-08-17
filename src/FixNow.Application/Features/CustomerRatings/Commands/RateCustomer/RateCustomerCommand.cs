using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerRatings.Commands.RateCustomer;

public sealed record RateCustomerCommand(
    Guid JobId,
    int Rating,
    string? Comment = null)
    : ICommand<Result<RateCustomerResponse>>;
