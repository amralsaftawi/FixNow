using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;

public sealed class GetAvailableServiceRequestsQueryValidator
    : AbstractValidator<GetAvailableServiceRequestsQuery>
{
    public GetAvailableServiceRequestsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
