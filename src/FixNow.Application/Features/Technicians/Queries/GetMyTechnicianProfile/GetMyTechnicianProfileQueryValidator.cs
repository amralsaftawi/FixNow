using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianProfile;

public sealed class GetMyTechnicianProfileQueryValidator
    : AbstractValidator<GetMyTechnicianProfileQuery>
{
    public GetMyTechnicianProfileQueryValidator()
    {
    }
}