using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobStarted;

public sealed class MarkJobStartedCommandValidator
    : AbstractValidator<MarkJobStartedCommand>
{
    public MarkJobStartedCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
