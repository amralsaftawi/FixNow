using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerPaymentMethod;

public sealed record RemoveCustomerPaymentMethodCommand(
    Guid PaymentMethodId)
    : ICommand<Result<Success>>;
