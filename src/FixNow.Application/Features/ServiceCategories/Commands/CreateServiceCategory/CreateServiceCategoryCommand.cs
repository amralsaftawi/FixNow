using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public sealed record CreateServiceCategoryCommand(
    string Name,
    string Description,
    string IconKey,
    int DisplayOrder)
    : ICommand<Result<CreateServiceCategoryResponse>>;