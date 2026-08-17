using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerPaymentMethod;

public sealed record AddCustomerPaymentMethodCommand(
    PaymentMethod Type)
    : ICommand<Result<Created>>;
