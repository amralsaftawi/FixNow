using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateMyTechnicianPersonalInformation;

public sealed record UpdateMyTechnicianPersonalInformationCommand(
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber,
    string CountryCode,
    PreferredLanguage PreferredLanguage)
    : ICommand<Result<TechnicianPersonalInformationResponse>>;
