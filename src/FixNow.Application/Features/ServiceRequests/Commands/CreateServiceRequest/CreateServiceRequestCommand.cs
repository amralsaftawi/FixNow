using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest;

public sealed record CreateServiceRequestCommand(
    Guid AddressId,
    Guid ServiceCategoryId,
    string Description,
    ServicePriority Priority,
    DateTimeOffset? ScheduledAt)
    : ICommand<Result<CreateServiceRequestResponse>>;
