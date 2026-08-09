using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryPricing;

public sealed record ConfigureServiceCategoryPricingCommand(
    Guid ServiceCategoryId,
    decimal Amount,
    Currency Currency)
    : ICommand<Result<Updated>>;
