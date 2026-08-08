namespace FixNow.Application.Features.GeographicData.Dtos.Responses;

public sealed record CityResponse(
    int CityId,
    int CountryId,
    string Name);
