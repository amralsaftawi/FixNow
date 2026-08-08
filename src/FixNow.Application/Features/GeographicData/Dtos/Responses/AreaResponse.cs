namespace FixNow.Application.Features.GeographicData.Dtos.Responses;

public sealed record AreaResponse(
    int AreaId,
    int CityId,
    string Name);
