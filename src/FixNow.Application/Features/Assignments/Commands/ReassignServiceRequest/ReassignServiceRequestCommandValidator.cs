using FluentValidation;

namespace FixNow.Application.Features.Assignments.Commands.ReassignServiceRequest;

public sealed class ReassignServiceRequestCommandValidator
    : AbstractValidator<ReassignServiceRequestCommand>
{
    public ReassignServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianProfile.IdRequired");
    }
}
