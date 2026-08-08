using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

namespace FixNow.Application.Features.CustomerProfiles.Queries.GetMyCustomerProfile;

public sealed record GetMyCustomerProfileQuery
    : IQuery<Result<CustomerProfileResponse>>;
