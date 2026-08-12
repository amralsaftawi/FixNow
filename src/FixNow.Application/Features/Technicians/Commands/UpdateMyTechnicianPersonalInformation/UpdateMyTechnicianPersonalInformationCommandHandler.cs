using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateMyTechnicianPersonalInformation;

public sealed class UpdateMyTechnicianPersonalInformationCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IUserRepository userRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateMyTechnicianPersonalInformationCommand, Result<TechnicianPersonalInformationResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IUserRepository _userRepository = userRepository;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<TechnicianPersonalInformationResponse>> Handle(
        UpdateMyTechnicianPersonalInformationCommand command,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var user = await _userRepository.GetByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        // 1. Update the name when it changed.
        if (user.FirstName != command.FirstName.Trim()
            || user.LastName != command.LastName.Trim())
        {
            var nameResult = user.ChangeName(
                command.FirstName,
                command.LastName);

            if (nameResult.IsError)
            {
                return nameResult.Errors;
            }
        }

        // 2. Update the email when a new one is provided.
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailResult = Email.Create(command.Email);

            if (emailResult.IsError)
            {
                return emailResult.Errors;
            }

            var newEmail = emailResult.Value;

            if (newEmail != user.Email)
            {
                if (await _userRepository.ExistsByEmailAsync(
                    newEmail,
                    cancellationToken))
                {
                    return IdentityErrors.EmailAlreadyExists;
                }

                var changeEmailResult = user.ChangeEmail(newEmail);

                if (changeEmailResult.IsError)
                {
                    return changeEmailResult.Errors;
                }
            }
        }

        // 3. Update the phone number when it changed.
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

        if (phoneNumberResult.IsError)
        {
            return phoneNumberResult.Errors;
        }

        var newPhoneNumber = phoneNumberResult.Value;

        if (newPhoneNumber != user.PhoneNumber)
        {
            if (await _userRepository.ExistsByPhoneNumberAsync(
                newPhoneNumber,
                cancellationToken))
            {
                return IdentityErrors.PhoneNumberAlreadyExists;
            }

            var changePhoneNumberResult = user.ChangePhoneNumber(
                newPhoneNumber);

            if (changePhoneNumberResult.IsError)
            {
                return changePhoneNumberResult.Errors;
            }
        }

        // 4. Update the country code when it changed.
        var countryCodeResult = CountryCode.Create(command.CountryCode);

        if (countryCodeResult.IsError)
        {
            return countryCodeResult.Errors;
        }

        var newCountryCode = countryCodeResult.Value;

        if (newCountryCode != user.CountryCode)
        {
            var changeCountryCodeResult = user.ChangeCountryCode(
                newCountryCode);

            if (changeCountryCodeResult.IsError)
            {
                return changeCountryCodeResult.Errors;
            }
        }

        // 5. Update the preferred language when it changed.
        if (user.PreferredLanguage != command.PreferredLanguage)
        {
            var languageResult = user.ChangeLanguage(
                command.PreferredLanguage);

            if (languageResult.IsError)
            {
                return languageResult.Errors;
            }
        }

        // 6. Persist the changes.
        _userRepository.Update(user);

        _technicianProfileRepository.Update(technicianProfile);

        // 7. Return the updated personal information.
        return user.ToTechnicianPersonalInformationResponse(technicianProfile);
    }
}
