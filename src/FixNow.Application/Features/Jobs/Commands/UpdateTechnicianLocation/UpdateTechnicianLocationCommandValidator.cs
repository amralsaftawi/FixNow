using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.UpdateTechnicianLocation;

public sealed class UpdateTechnicianLocationCommandValidator
    : AbstractValidator<UpdateTechnicianLocationCommand>
{
    public UpdateTechnicianLocationCommandValidator()
    {
        ValidateJobId();

        ValidateLatitude();

        ValidateLongitude();
    }

    private void ValidateJobId()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }

    private void ValidateLatitude()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithErrorCode("TechnicianProfile.LatitudeInvalid");
    }

    private void ValidateLongitude()
    {
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithErrorCode("TechnicianProfile.LongitudeInvalid");
    }
}
