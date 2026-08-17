using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobArrived;

public sealed class MarkJobArrivedCommandValidator
    : AbstractValidator<MarkJobArrivedCommand>
{
    public MarkJobArrivedCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
