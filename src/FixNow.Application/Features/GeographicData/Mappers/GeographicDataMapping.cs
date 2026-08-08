using FixNow.Application.Features.GeographicData.Dtos.Responses;

namespace FixNow.Application.Features.GeographicData.Mappers;

public static class GeographicDataMapping
{
    public static CountryResponse ToResponse(
        this Country entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CountryResponse(
            CountryId: entity.Id,
            Name: entity.Name);
    }

    public static List<CountryResponse> ToResponses(
        this IEnumerable<Country> entities)
    {
        return entities.Select(ToResponse).ToList();
    }

    public static CityResponse ToResponse(
        this City entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CityResponse(
            CityId: entity.Id,
            CountryId: entity.CountryId,
            Name: entity.Name);
    }

    public static List<CityResponse> ToResponses(
        this IEnumerable<City> entities)
    {
        return entities.Select(ToResponse).ToList();
    }

    public static AreaResponse ToResponse(
        this Area entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AreaResponse(
            AreaId: entity.Id,
            CityId: entity.CityId,
            Name: entity.Name);
    }

    public static List<AreaResponse> ToResponses(
        this IEnumerable<Area> entities)
    {
        return entities.Select(ToResponse).ToList();
    }
}
