using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectServiceCategory;

public sealed record SelectServiceCategoryCommand(
    Guid ServiceRequestId,
    Guid ServiceCategoryId)
    : ICommand<Result<Success>>;
