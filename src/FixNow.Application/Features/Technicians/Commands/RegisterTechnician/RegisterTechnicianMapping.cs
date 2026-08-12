namespace FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;

public static class RegisterTechnicianMapping
{
    public static RegisterTechnicianResponse ToRegisterTechnicianResponse(
        this TechnicianProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new RegisterTechnicianResponse(
            TechnicianProfileId: entity.Id,
            UserId: entity.UserId,
            VerificationStatus: entity.VerificationStatus,
            Availability: entity.Availability,
            YearsOfExperience: entity.YearsOfExperience,
            Bio: entity.Bio,
            NationalIdImageKey: entity.NationalIdImageKey,
            IsProfileCompleted: entity.IsProfileCompleted);
    }
}
