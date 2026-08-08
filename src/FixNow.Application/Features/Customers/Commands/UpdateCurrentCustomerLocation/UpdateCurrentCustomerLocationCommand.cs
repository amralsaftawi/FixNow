using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.UpdateCurrentCustomerLocation;

public sealed record UpdateCurrentCustomerLocationCommand(
    decimal Latitude,
    decimal Longitude)
    : ICommand<Result<Success>>;
