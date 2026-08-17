using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.RateTechnician;

public sealed class RateTechnicianCommandValidator
    : AbstractValidator<RateTechnicianCommand>
{
    public RateTechnicianCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithErrorCode("Rating.InvalidValue");
    }
}
