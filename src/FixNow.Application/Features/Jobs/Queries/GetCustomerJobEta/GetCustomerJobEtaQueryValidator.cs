using FluentValidation;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;

public sealed class GetCustomerJobEtaQueryValidator
    : AbstractValidator<GetCustomerJobEtaQuery>
{
    public GetCustomerJobEtaQueryValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
