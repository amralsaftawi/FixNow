using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.SetPreferredServiceTime;

public sealed class SetPreferredServiceTimeCommandValidator
    : AbstractValidator<SetPreferredServiceTimeCommand>
{
    public SetPreferredServiceTimeCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.PreferredServiceTime)
            .Must(value => value > DateTimeOffset.UtcNow)
            .WithErrorCode("ServiceRequest.InvalidScheduleDate");
    }
}
