using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;

public sealed class UpdateTechnicianAvailabilityCommandValidator
    : AbstractValidator<UpdateTechnicianAvailabilityCommand>
{
    public UpdateTechnicianAvailabilityCommandValidator()
    {
        RuleFor(x => x.Availability)
            .IsInEnum()
            .WithErrorCode("TechnicianProfile.Availability.Invalid");
    }
}
