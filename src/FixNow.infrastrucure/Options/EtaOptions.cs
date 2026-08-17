namespace FixNow.Infrastructure.Options;

public sealed class EtaOptions
{
    public const string SectionName = "Eta";

    public double EstimatedTravelSpeedKmh { get; init; } = 30.0;
}
