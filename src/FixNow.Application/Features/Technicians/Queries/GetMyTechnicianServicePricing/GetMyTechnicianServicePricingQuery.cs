using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianServicePricing;

public sealed record GetMyTechnicianServicePricingQuery
    : IQuery<Result<List<TechnicianServicePricingResponse>>>;
