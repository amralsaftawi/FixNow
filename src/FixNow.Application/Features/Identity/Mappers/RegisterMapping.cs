

using FixNow.Application.Features.Identity.Commands.Register;

public static class RegisterMapping
{
    public static RegisterResponse ToRegisterResponse(this User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new RegisterResponse(
            UserId: entity.Id,
            Email: entity.Email.Value);
    }
}