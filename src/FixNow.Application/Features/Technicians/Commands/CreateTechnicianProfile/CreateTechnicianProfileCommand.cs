using FixNow.Application.Common.Abstractions.Messaging;

public sealed record CreateTechnicianProfileCommand(
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey)
    : ICommand<Result<Created>>;