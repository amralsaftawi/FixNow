using FluentValidation;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobTracking;

public sealed class GetCustomerJobTrackingQueryValidator
    : AbstractValidator<GetCustomerJobTrackingQuery>
{
    public GetCustomerJobTrackingQueryValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
