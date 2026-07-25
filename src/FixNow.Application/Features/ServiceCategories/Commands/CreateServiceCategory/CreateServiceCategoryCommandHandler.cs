
using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public sealed class CreateServiceCategoryCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<
        CreateServiceCategoryCommand,
        Result<CreateServiceCategoryResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<CreateServiceCategoryResponse>> Handle(
        CreateServiceCategoryCommand command,
        CancellationToken cancellationToken)
    {
        if (await _serviceCategoryRepository.ExistsByNameAsync(command.Name,cancellationToken))
        {
            return ServiceCategoryErrors.NameAlreadyExists;
        }

        var createServiceCategoryResult = ServiceCategory.Create(
            id: Guid.NewGuid(),
            name: command.Name,
            description: command.Description,
            iconKey: command.IconKey,
            displayOrder: command.DisplayOrder);

        if (createServiceCategoryResult.IsError)
        {
            return createServiceCategoryResult.Errors;
        }

        var serviceCategory = createServiceCategoryResult.Value;

        await _serviceCategoryRepository.AddAsync(serviceCategory,cancellationToken);

        return serviceCategory.ToCreateServiceCategoryResponse();
    }
}