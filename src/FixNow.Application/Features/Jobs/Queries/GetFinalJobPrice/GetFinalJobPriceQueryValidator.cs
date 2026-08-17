using FluentValidation;

namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed class GetFinalJobPriceQueryValidator
    : AbstractValidator<GetFinalJobPriceQuery>
{
    public GetFinalJobPriceQueryValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
