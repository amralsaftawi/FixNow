using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianServicePricing;

public sealed record UpdateTechnicianServicePricingCommand(
    Guid TechnicianServiceId,
    decimal Amount,
    Currency Currency)
    : ICommand<Result<TechnicianServicePricingResponse>>;
