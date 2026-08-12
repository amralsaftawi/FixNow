using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianService;

public sealed record AddTechnicianServiceCommand(
    Guid ServiceCategoryId)
    : ICommand<Result<TechnicianServiceResponse>>;
