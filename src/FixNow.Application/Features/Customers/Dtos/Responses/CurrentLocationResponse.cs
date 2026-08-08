namespace FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

public sealed record CurrentLocationResponse(
    decimal Latitude,
    decimal Longitude,
    DateTimeOffset UpdatedAtUtc);
