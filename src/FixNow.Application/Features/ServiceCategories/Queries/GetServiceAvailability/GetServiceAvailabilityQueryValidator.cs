using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceAvailability;

public sealed class GetServiceAvailabilityQueryValidator
    : AbstractValidator<GetServiceAvailabilityQuery>
{
    public GetServiceAvailabilityQueryValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");
    }
}
