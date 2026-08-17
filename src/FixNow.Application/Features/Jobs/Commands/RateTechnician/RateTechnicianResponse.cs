namespace FixNow.Application.Features.Jobs.Commands.RateTechnician;

public sealed record RateTechnicianResponse(
    Guid ReviewId,
    int Rating);
