using FluentValidation;

namespace FixNow.Application.Features.Assignments.Commands.AssignServiceRequest;

public sealed class AssignServiceRequestCommandValidator
    : AbstractValidator<AssignServiceRequestCommand>
{
    public AssignServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianProfile.IdRequired");
    }
}
