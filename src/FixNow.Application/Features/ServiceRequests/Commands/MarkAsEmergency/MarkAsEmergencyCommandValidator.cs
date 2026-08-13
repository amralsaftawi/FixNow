using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.MarkAsEmergency;

public sealed class MarkAsEmergencyCommandValidator
    : AbstractValidator<MarkAsEmergencyCommand>
{
    public MarkAsEmergencyCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
