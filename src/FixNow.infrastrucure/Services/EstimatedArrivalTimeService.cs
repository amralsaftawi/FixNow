using FixNow.Application.Common.Interfaces.Services;
using FixNow.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FixNow.Infrastructure.Services;

public sealed class EstimatedArrivalTimeService(IOptions<EtaOptions> etaOptions)
    : IEstimatedArrivalTimeService
{
    private const double EarthRadiusKm = 6371.0;

    private readonly double _estimatedTravelSpeedKmh =
        etaOptions.Value.EstimatedTravelSpeedKmh;

    public EtaEstimate? Estimate(
        decimal technicianLatitude,
        decimal technicianLongitude,
        decimal destinationLatitude,
        decimal destinationLongitude,
        DateTimeOffset referenceTimeUtc)
    {
        if (_estimatedTravelSpeedKmh <= 0)
            return null;

        if (!IsValidCoordinate(technicianLatitude, technicianLongitude) ||
            !IsValidCoordinate(destinationLatitude, destinationLongitude))
        {
            return null;
        }

        var distanceKm = HaversineDistanceKm(
            (double)technicianLatitude,
            (double)technicianLongitude,
            (double)destinationLatitude,
            (double)destinationLongitude);

        var travelHours = distanceKm / _estimatedTravelSpeedKmh;

        var travelMinutes = (int)Math.Ceiling(travelHours * 60.0);

        var estimatedArrivalTimeUtc = referenceTimeUtc.AddMinutes(travelMinutes);

        return new EtaEstimate(
            DistanceKm: Math.Round(distanceKm, 2),
            EstimatedTravelMinutes: travelMinutes,
            EstimatedArrivalTimeUtc: estimatedArrivalTimeUtc);
    }

    private static bool IsValidCoordinate(
        decimal latitude,
        decimal longitude)
        => latitude is >= -90m and <= 90m
           && longitude is >= -180m and <= 180m;

    private static double HaversineDistanceKm(
        double lat1,
        double lon1,
        double lat2,
        double lon2)
    {
        var lat1Rad = DegreesToRadians(lat1);
        var lat2Rad = DegreesToRadians(lat2);
        var deltaLat = DegreesToRadians(lat2 - lat1);
        var deltaLon = DegreesToRadians(lon2 - lon1);

        var a =
            Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
            Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
            Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double DegreesToRadians(double degrees)
        => degrees * Math.PI / 180.0;
}
