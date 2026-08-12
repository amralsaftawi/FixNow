using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;

public sealed class GetTechnicianProfilesQueryValidator
    : AbstractValidator<GetTechnicianProfilesQuery>
{
    public GetTechnicianProfilesQueryValidator()
    {
        ValidateVerificationStatus();

        ValidatePageNumber();

        ValidatePageSize();
    }

    private void ValidateVerificationStatus()
    {
        RuleFor(x => x.VerificationStatus)
            .IsInEnum()
            .WithErrorCode("TechnicianProfile.VerificationStatus.Invalid");
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
