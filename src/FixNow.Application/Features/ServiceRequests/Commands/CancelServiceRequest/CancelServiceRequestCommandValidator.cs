using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.CancelServiceRequest;

public sealed class CancelServiceRequestCommandValidator
    : AbstractValidator<CancelServiceRequestCommand>
{
    public CancelServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
