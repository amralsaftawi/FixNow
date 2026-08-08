using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerAddress;

public sealed record SetDefaultCustomerAddressCommand(
    Guid AddressId)
    : ICommand<Result<Success>>;
