using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;

public sealed class GetTechnicianJobHistoryQueryValidator
    : AbstractValidator<GetTechnicianJobHistoryQuery>
{
    public GetTechnicianJobHistoryQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
