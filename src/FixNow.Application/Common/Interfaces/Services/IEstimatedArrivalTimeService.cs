namespace FixNow.Application.Common.Interfaces.Services;

public sealed record EtaEstimate(
    double DistanceKm,
    int EstimatedTravelMinutes,
    DateTimeOffset EstimatedArrivalTimeUtc);

public interface IEstimatedArrivalTimeService
{
    EtaEstimate? Estimate(
        decimal technicianLatitude,
        decimal technicianLongitude,
        decimal destinationLatitude,
        decimal destinationLongitude,
        DateTimeOffset referenceTimeUtc);
}
