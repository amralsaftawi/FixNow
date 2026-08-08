using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;
using FixNow.Application.Features.CustomerProfiles.Mappers;

namespace FixNow.Application.Features.CustomerProfiles.Queries.GetMyCustomerProfile;

public sealed class GetMyCustomerProfileQueryHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyCustomerProfileQuery, Result<CustomerProfileResponse>>
{
    public async Task<Result<CustomerProfileResponse>> Handle(
        GetMyCustomerProfileQuery query,
        CancellationToken cancellationToken)
    {
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        return customerProfile.ToCustomerProfileResponse();
    }
}
