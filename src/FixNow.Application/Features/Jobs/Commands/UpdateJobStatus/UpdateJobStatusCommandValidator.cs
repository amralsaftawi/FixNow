using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.UpdateJobStatus;

public sealed class UpdateJobStatusCommandValidator
    : AbstractValidator<UpdateJobStatusCommand>
{
    public UpdateJobStatusCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithErrorCode("Job.InvalidStatus");
    }
}
