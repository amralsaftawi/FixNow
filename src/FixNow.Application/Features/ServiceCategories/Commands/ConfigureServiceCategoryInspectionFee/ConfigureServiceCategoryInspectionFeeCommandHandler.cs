using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryInspectionFee;

public sealed class ConfigureServiceCategoryInspectionFeeCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<ConfigureServiceCategoryInspectionFeeCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(
        ConfigureServiceCategoryInspectionFeeCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var inspectionFeeResult = Money.Create(
            command.Amount,
            command.Currency);

        if (inspectionFeeResult.IsError)
        {
            return inspectionFeeResult.Errors;
        }

        var setInspectionFeeResult = serviceCategory.SetInspectionFee(
            inspectionFeeResult.Value);

        if (setInspectionFeeResult.IsError)
        {
            return setInspectionFeeResult.Errors;
        }

        return Result.Updated;
    }
}
