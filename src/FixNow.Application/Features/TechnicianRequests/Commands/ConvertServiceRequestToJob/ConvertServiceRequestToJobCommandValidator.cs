using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Commands.ConvertServiceRequestToJob;

public sealed class ConvertServiceRequestToJobCommandValidator
    : AbstractValidator<ConvertServiceRequestToJobCommand>
{
    public ConvertServiceRequestToJobCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
