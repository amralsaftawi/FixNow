using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerAddress;

public sealed record RemoveCustomerAddressCommand(
    Guid AddressId)
    : ICommand<Result<Success>>;
