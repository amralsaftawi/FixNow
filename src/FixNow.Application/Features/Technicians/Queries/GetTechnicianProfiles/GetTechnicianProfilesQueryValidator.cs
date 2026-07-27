using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;

public sealed class GetTechnicianProfilesQueryValidator
    : AbstractValidator<GetTechnicianProfilesQuery>
{
    public GetTechnicianProfilesQueryValidator()
    {
        ValidatePageNumber();
        ValidatePageSize();
    }

    private void ValidatePageNumber()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");
    }

    private void ValidatePageSize()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}