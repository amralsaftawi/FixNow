using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryPricing;

public sealed class ConfigureServiceCategoryPricingCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<ConfigureServiceCategoryPricingCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(
        ConfigureServiceCategoryPricingCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var priceResult = Money.Create(
            command.Amount,
            command.Currency);

        if (priceResult.IsError)
        {
            return priceResult.Errors;
        }

        var setPriceResult = serviceCategory.SetPrice(priceResult.Value);

        if (setPriceResult.IsError)
        {
            return setPriceResult.Errors;
        }

        return Result.Updated;
    }
}
