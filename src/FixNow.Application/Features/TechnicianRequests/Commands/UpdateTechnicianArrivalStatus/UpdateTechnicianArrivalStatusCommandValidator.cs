using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Commands.UpdateTechnicianArrivalStatus;

public sealed class UpdateTechnicianArrivalStatusCommandValidator
    : AbstractValidator<UpdateTechnicianArrivalStatusCommand>
{
    public UpdateTechnicianArrivalStatusCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithErrorCode("ServiceRequest.InvalidArrivalStatus");
    }
}
