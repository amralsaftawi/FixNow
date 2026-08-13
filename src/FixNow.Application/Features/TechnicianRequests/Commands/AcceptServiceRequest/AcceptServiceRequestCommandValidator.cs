using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Commands.AcceptServiceRequest;

public sealed class AcceptServiceRequestCommandValidator
    : AbstractValidator<AcceptServiceRequestCommand>
{
    public AcceptServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
