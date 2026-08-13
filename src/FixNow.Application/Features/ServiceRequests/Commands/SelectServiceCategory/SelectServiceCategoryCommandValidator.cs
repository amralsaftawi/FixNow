using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectServiceCategory;

public sealed class SelectServiceCategoryCommandValidator
    : AbstractValidator<SelectServiceCategoryCommand>
{
    public SelectServiceCategoryCommandValidator()
    {
        ValidateServiceRequestId();

        ValidateServiceCategoryId();
    }

    private void ValidateServiceRequestId()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }

    private void ValidateServiceCategoryId()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.ServiceCategoryIdRequired");
    }
}
