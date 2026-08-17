using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobEnRoute;

public sealed class MarkJobEnRouteCommandValidator
    : AbstractValidator<MarkJobEnRouteCommand>
{
    public MarkJobEnRouteCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
