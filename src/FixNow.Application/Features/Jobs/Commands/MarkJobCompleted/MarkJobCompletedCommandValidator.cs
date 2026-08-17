using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobCompleted;

public sealed class MarkJobCompletedCommandValidator
    : AbstractValidator<MarkJobCompletedCommand>
{
    public MarkJobCompletedCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
