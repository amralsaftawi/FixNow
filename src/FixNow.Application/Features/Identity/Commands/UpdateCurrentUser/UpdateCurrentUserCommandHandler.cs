using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommandHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCurrentUserCommand, Result<UpdateCurrentUserResponse>>
{
    public async Task<Result<UpdateCurrentUserResponse>> Handle(
        UpdateCurrentUserCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Ensure the user is authenticated.
        if (!currentUser.IsAuthenticated)
            return IdentityErrors.Unauthorized;

        // 2. Find the current user.
        var user = await userRepository.GetByIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        // 3. Update the name when it changed.
        if (user.FirstName != command.FirstName.Trim()
            || user.LastName != command.LastName.Trim())
        {
            var nameResult = user.ChangeName(
                command.FirstName,
                command.LastName);

            if (nameResult.IsError)
                return nameResult.Errors;
        }

        // 4. Update the email when a new one is provided.
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailResult = Email.Create(command.Email);

            if (emailResult.IsError)
                return emailResult.Errors;

            var newEmail = emailResult.Value;

            if (newEmail != user.Email)
            {
                if (await userRepository.ExistsByEmailAsync(
                    newEmail,
                    cancellationToken))
                {
                    return IdentityErrors.EmailAlreadyExists;
                }

                var changeEmailResult = user.ChangeEmail(newEmail);

                if (changeEmailResult.IsError)
                    return changeEmailResult.Errors;
            }
        }

        // 5. Update the phone number when it changed.
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

        if (phoneNumberResult.IsError)
            return phoneNumberResult.Errors;

        var newPhoneNumber = phoneNumberResult.Value;

        if (newPhoneNumber != user.PhoneNumber)
        {
            if (await userRepository.ExistsByPhoneNumberAsync(
                newPhoneNumber,
                cancellationToken))
            {
                return IdentityErrors.PhoneNumberAlreadyExists;
            }

            var changePhoneNumberResult = user.ChangePhoneNumber(
                newPhoneNumber);

            if (changePhoneNumberResult.IsError)
                return changePhoneNumberResult.Errors;
        }

        // 6. Update the country code when it changed.
        var countryCodeResult = CountryCode.Create(command.CountryCode);

        if (countryCodeResult.IsError)
            return countryCodeResult.Errors;

        var newCountryCode = countryCodeResult.Value;

        if (newCountryCode != user.CountryCode)
        {
            var changeCountryCodeResult = user.ChangeCountryCode(
                newCountryCode);

            if (changeCountryCodeResult.IsError)
                return changeCountryCodeResult.Errors;
        }

        // 7. Update the preferred language when it changed.
        if (user.PreferredLanguage != command.PreferredLanguage)
        {
            var languageResult = user.ChangeLanguage(
                command.PreferredLanguage);

            if (languageResult.IsError)
                return languageResult.Errors;
        }

        // 8. Persist the changes.
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 9. Return the updated profile.
        return new UpdateCurrentUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email?.Value,
            user.PhoneNumber.Value);
    }
}
