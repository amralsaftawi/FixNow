using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Commands.RejectServiceRequest;

public sealed class RejectServiceRequestCommandValidator
    : AbstractValidator<RejectServiceRequestCommand>
{
    public RejectServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.Reason)
            .IsInEnum()
            .WithErrorCode("Assignment.RejectReasonRequired");
    }
}
