namespace FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

public sealed record CustomerProfileResponse(
    Guid CustomerProfileId,
    Guid UserId,
    DateTimeOffset RegisteredAt,
    IReadOnlyCollection<AddressResponse> Addresses);
