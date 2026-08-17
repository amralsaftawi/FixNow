using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobPaused;

public sealed class MarkJobPausedCommandValidator
    : AbstractValidator<MarkJobPausedCommand>
{
    public MarkJobPausedCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
