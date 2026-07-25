

using FixNow.Application.Common.Abstractions.Messaging;

public sealed record DeactivateServiceCategoryCommand(Guid ServiceCategoryId)
    : ICommand<Result<Updated>>;