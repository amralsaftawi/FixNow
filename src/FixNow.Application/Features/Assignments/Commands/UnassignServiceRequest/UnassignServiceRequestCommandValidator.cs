using FluentValidation;

namespace FixNow.Application.Features.Assignments.Commands.UnassignServiceRequest;

public sealed class UnassignServiceRequestCommandValidator
    : AbstractValidator<UnassignServiceRequestCommand>
{
    public UnassignServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
