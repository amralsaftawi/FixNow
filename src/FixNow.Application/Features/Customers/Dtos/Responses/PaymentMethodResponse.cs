namespace FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

public sealed record PaymentMethodResponse(
    Guid PaymentMethodId,
    PaymentMethod Type,
    bool IsDefault);
