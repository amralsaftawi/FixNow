using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators;

public sealed class GetTechnicianTrustIndicatorsQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IUserRepository userRepository,
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<GetTechnicianTrustIndicatorsQuery, Result<GetTechnicianTrustIndicatorsResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<GetTechnicianTrustIndicatorsResponse>> Handle(
        GetTechnicianTrustIndicatorsQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByIdAsync(
            query.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var user = await _userRepository.GetByIdAsync(
            technicianProfile.UserId,
            cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        var ratingSummary = await _technicianDiscoveryRepository
            .GetRatingSummaryByTechnicianAsync(
                query.TechnicianProfileId,
                cancellationToken);

        return new GetTechnicianTrustIndicatorsResponse(
            TechnicianProfileId: query.TechnicianProfileId,
            IsVerified: technicianProfile.VerificationStatus == VerificationStatus.Verified,
            IsProfileComplete: technicianProfile.IsProfileCompleted,
            IsActive: user.AccountStatus == AccountStatus.Active,
            YearsOfExperience: technicianProfile.YearsOfExperience,
            AverageRating: ratingSummary?.AverageRating ?? 0,
            TotalRatings: ratingSummary?.RatingCount ?? 0);
    }
}
