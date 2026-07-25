using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.ActivateServiceCategory;

public sealed record ActivateServiceCategoryCommand(
    Guid ServiceCategoryId)
    : ICommand<Result<Updated>>;