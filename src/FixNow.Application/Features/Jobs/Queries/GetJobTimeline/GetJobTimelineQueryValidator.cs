using FluentValidation;

namespace FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public sealed class GetJobTimelineQueryValidator
    : AbstractValidator<GetJobTimelineQuery>
{
    public GetJobTimelineQueryValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
