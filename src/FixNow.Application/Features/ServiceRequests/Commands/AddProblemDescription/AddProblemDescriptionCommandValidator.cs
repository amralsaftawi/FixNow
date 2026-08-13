using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.AddProblemDescription;

public sealed class AddProblemDescriptionCommandValidator
    : AbstractValidator<AddProblemDescriptionCommand>
{
    public AddProblemDescriptionCommandValidator()
    {
        ValidateServiceRequestId();

        ValidateDescription();
    }

    private void ValidateServiceRequestId()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }

    private void ValidateDescription()
    {
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.DescriptionRequired")
            .MaximumLength(2000)
            .WithErrorCode("ServiceRequest.DescriptionTooLong");
    }
}
