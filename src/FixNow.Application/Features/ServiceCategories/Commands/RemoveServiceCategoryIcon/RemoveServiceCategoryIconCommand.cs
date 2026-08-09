using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.RemoveServiceCategoryIcon;

public sealed record RemoveServiceCategoryIconCommand(
    Guid ServiceCategoryId)
    : ICommand<Result<Updated>>;
