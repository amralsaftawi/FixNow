
public sealed class User : AuditableEntity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public Email? Email { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    public CountryCode CountryCode { get; private set; }

    public PreferredLanguage PreferredLanguage { get; private set; }

    public AuthProvider RegisteredVia { get; private set; }

    public AccountStatus AccountStatus { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public bool IsPhoneNumberVerified { get; private set; }

    public string? ProfileImageKey { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning disable CS8618
    private User(
        Guid id,
        string firstName,
        string lastName,
        Email? email,
        PhoneNumber phoneNumber,
        PasswordHash passwordHash,
        CountryCode countryCode,
        PreferredLanguage preferredLanguage,
        AuthProvider registeredVia,
        string? profileImageKey)
        : base(id)
    {
        FirstName = firstName;

        LastName = lastName;

        Email = email;

        PhoneNumber = phoneNumber;

        PasswordHash = passwordHash;

        CountryCode = countryCode;

        PreferredLanguage = preferredLanguage;

        RegisteredVia = registeredVia;

        ProfileImageKey = profileImageKey;

        AccountStatus = AccountStatus.PendingVerification;

        IsEmailVerified = false;

        IsPhoneNumberVerified = false;
    }

    public static Result<User> Create(
    Guid id,
    string firstName,
    string lastName,
    Email? email,
    PhoneNumber phoneNumber,
    PasswordHash passwordHash,
    CountryCode countryCode,
    PreferredLanguage preferredLanguage,
    AuthProvider registeredVia,
    string? profileImageKey = null)
{
    if (id == Guid.Empty)
        return UserErrors.IdRequired;

    if (string.IsNullOrWhiteSpace(firstName))
        return UserErrors.FirstNameRequired;

    if (string.IsNullOrWhiteSpace(lastName))
        return UserErrors.LastNameRequired;

    firstName = firstName.Trim();
    lastName = lastName.Trim();

    if (firstName.Length > 100)
        return UserErrors.FirstNameTooLong;

    if (lastName.Length > 100)
        return UserErrors.LastNameTooLong;

    var user = new User(
        id,
        firstName,
        lastName,
        email,
        phoneNumber,
        passwordHash,
        countryCode,
        preferredLanguage,
        registeredVia,
        profileImageKey);

    user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id));

    return user;
}

 public Result<Success> VerifyEmail()
{
    if (IsEmailVerified)
        return UserErrors.EmailAlreadyVerified;

    IsEmailVerified = true;

    AddDomainEvent(new UserEmailVerifiedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> VerifyPhoneNumber()
{
    if (IsPhoneNumberVerified)
        return UserErrors.PhoneAlreadyVerified;

    IsPhoneNumberVerified = true;

    AddDomainEvent(new UserPhoneVerifiedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> ChangePassword(PasswordHash newPasswordHash)
{
    if (PasswordHash == newPasswordHash)
        return UserErrors.SamePassword;

    PasswordHash = newPasswordHash;

    AddDomainEvent(new UserPasswordChangedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> ChangeLanguage(PreferredLanguage language)
{
    if (PreferredLanguage == language)
        return UserErrors.SameLanguage;

    PreferredLanguage = language;

    AddDomainEvent(new UserLanguageChangedDomainEvent(Id, language));

    return Result.Success;
}

public Result<Success> ChangeProfileImage(string? profileImageKey)
{
    if (ProfileImageKey == profileImageKey)
        return UserErrors.SameProfileImage;

    ProfileImageKey = profileImageKey;

    AddDomainEvent(new UserProfileImageChangedDomainEvent(Id, profileImageKey));

    return Result.Success;
}

public Result<Success> Activate()
{
    if (AccountStatus == AccountStatus.Active)
        return UserErrors.AlreadyActive;

    AccountStatus = AccountStatus.Active;

    AddDomainEvent(new UserActivatedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> Suspend()
{
    if (AccountStatus == AccountStatus.Suspended)
        return UserErrors.AlreadySuspended;

    AccountStatus = AccountStatus.Suspended;

    AddDomainEvent(new UserSuspendedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> Deactivate()
{
    if (AccountStatus == AccountStatus.Deactivated)
        return UserErrors.AlreadyDeactivated;

    AccountStatus = AccountStatus.Deactivated;

    AddDomainEvent(new UserDeactivatedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> SoftDelete()
{
    if (AccountStatus == AccountStatus.Deleted)
        return UserErrors.AlreadyDeleted;

    AccountStatus = AccountStatus.Deleted;

    DeletedAt = DateTimeOffset.UtcNow;

    AddDomainEvent(new UserDeletedDomainEvent(Id,  DeletedAt!.Value));

    return Result.Success;
}
}

