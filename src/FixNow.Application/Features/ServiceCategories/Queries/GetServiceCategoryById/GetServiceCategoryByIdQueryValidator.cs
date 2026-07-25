using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public sealed class GetServiceCategoryByIdQueryValidator
    : AbstractValidator<GetServiceCategoryByIdQuery>
{
    public GetServiceCategoryByIdQueryValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");
    }
}