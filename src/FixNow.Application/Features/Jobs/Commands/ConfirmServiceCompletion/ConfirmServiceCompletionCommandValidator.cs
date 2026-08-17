using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.ConfirmServiceCompletion;

public sealed class ConfirmServiceCompletionCommandValidator
    : AbstractValidator<ConfirmServiceCompletionCommand>
{
    public ConfirmServiceCompletionCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
