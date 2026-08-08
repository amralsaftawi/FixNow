using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.UpdateCurrentCustomerLocation;

public sealed class UpdateCurrentCustomerLocationCommandValidator
    : AbstractValidator<UpdateCurrentCustomerLocationCommand>
{
    public UpdateCurrentCustomerLocationCommandValidator()
    {
        ValidateLatitude();

        ValidateLongitude();
    }

    private void ValidateLatitude()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithErrorCode("CustomerProfile.LatitudeInvalid");
    }

    private void ValidateLongitude()
    {
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithErrorCode("CustomerProfile.LongitudeInvalid");
    }
}
