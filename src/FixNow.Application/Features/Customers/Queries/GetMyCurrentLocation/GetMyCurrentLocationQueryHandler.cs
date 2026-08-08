using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

namespace FixNow.Application.Features.CustomerProfiles.Queries.GetMyCurrentLocation;

public sealed class GetMyCurrentLocationQueryHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyCurrentLocationQuery, Result<CurrentLocationResponse>>
{
    public async Task<Result<CurrentLocationResponse>> Handle(
        GetMyCurrentLocationQuery query,
        CancellationToken cancellationToken)
    {
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        if (customerProfile.CurrentLatitude is null
            || customerProfile.CurrentLongitude is null
            || customerProfile.CurrentLocationUpdatedAtUtc is null)
        {
            return CustomerProfileErrors.CurrentLocationNotFound;
        }

        return new CurrentLocationResponse(
            Latitude: customerProfile.CurrentLatitude.Value,
            Longitude: customerProfile.CurrentLongitude.Value,
            UpdatedAtUtc: customerProfile.CurrentLocationUpdatedAtUtc.Value);
    }
}
