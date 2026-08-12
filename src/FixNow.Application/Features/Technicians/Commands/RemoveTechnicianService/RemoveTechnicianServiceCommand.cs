using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianService;

public sealed record RemoveTechnicianServiceCommand(
    Guid ServiceCategoryId)
    : ICommand<Result<Success>>;
