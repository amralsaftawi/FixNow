using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.CompleteServiceRequest;

public sealed class CompleteServiceRequestCommandValidator
    : AbstractValidator<CompleteServiceRequestCommand>
{
    public CompleteServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
