using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.CancelJob;

public sealed class CancelJobCommandValidator
    : AbstractValidator<CancelJobCommand>
{
    public CancelJobCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
