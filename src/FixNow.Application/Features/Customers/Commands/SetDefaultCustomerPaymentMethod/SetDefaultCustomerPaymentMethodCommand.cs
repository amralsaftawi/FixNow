using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerPaymentMethod;

public sealed record SetDefaultCustomerPaymentMethodCommand(
    Guid PaymentMethodId)
    : ICommand<Result<Success>>;
