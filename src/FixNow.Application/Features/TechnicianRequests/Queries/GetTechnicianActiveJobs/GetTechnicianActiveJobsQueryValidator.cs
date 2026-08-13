using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;

public sealed class GetTechnicianActiveJobsQueryValidator
    : AbstractValidator<GetTechnicianActiveJobsQuery>
{
    public GetTechnicianActiveJobsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
