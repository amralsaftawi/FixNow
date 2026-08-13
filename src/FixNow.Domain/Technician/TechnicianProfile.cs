public sealed class TechnicianProfile : AuditableEntity
{
    public Guid UserId { get; private set; }

    public VerificationStatus VerificationStatus { get; private set; }

    public TechnicianAvailability Availability { get; private set; }

    public TechnicianAvailabilitySettings AvailabilitySettings { get; private set; } =
        null!;

    public int YearsOfExperience { get; private set; }

    public string? Bio { get; private set; }

    public string? NationalIdImageKey { get; private set; }

    public bool IsProfileCompleted { get; private set; }

    public decimal? Latitude { get; private set; }

    public decimal? Longitude { get; private set; }

    public int? CityId { get; private set; }

    // Navigation

    public User User { get; private set; } = null!;

    public City? City { get; private set; }

    private readonly List<TechnicianService> _services = [];

    public IReadOnlyCollection<TechnicianService> Services =>
        _services.AsReadOnly();

    private readonly List<TechnicianExperience> _experiences = [];

    public IReadOnlyCollection<TechnicianExperience> Experiences =>
        _experiences.AsReadOnly();

    private readonly List<TechnicianPortfolioItem> _portfolioItems = [];

    public IReadOnlyCollection<TechnicianPortfolioItem> PortfolioItems =>
        _portfolioItems.AsReadOnly();

#pragma warning disable CS8618
    private TechnicianProfile()
    {
    }
#pragma warning disable CS8618
    private TechnicianProfile(
        Guid id,
        Guid userId,
        int yearsOfExperience,
        string? bio,
        string? nationalIdImageKey)
        : base(id)
    {
        UserId = userId;

        YearsOfExperience = yearsOfExperience;

        Bio = bio;

        NationalIdImageKey = nationalIdImageKey;

        VerificationStatus = VerificationStatus.Pending;

        Availability = TechnicianAvailability.Offline;

        AvailabilitySettings = TechnicianAvailabilitySettings
            .Create(
                TechnicianAvailabilityStatus.NotAcceptingRequests,
                [],
                null,
                null)
            .Value;

        IsProfileCompleted = false;
    }


public static Result<TechnicianProfile> Create(
    Guid id,
    Guid userId,
    int yearsOfExperience,
    string? bio,
    string? nationalIdImageKey)
{
    if (id == Guid.Empty)
        return TechnicianProfileErrors.IdRequired;

    if (userId == Guid.Empty)
        return TechnicianProfileErrors.UserIdRequired;

    if (yearsOfExperience < 0)
        return TechnicianProfileErrors.InvalidYearsOfExperience;

    bio = bio?.Trim();

    if (bio?.Length > 1000)
        return TechnicianProfileErrors.BioTooLong;

    nationalIdImageKey = nationalIdImageKey?.Trim();

    var technician = new TechnicianProfile(
        id,
        userId,
        yearsOfExperience,
        bio,
        nationalIdImageKey);

    technician.AddDomainEvent(
        new TechnicianProfileCreatedDomainEvent(
            technician.Id,
            technician.UserId));

    return technician;
}




public Result<Success> Verify()
{
    if (VerificationStatus == VerificationStatus.Verified)
        return TechnicianProfileErrors.AlreadyVerified;

    VerificationStatus = VerificationStatus.Verified;

    AddDomainEvent(
        new TechnicianVerifiedDomainEvent(
            Id,
            UserId));

    return Result.Success;
}


public Result<Success> RejectVerification()
{
    if (VerificationStatus == VerificationStatus.Rejected)
        return TechnicianProfileErrors.AlreadyRejected;

    VerificationStatus = VerificationStatus.Rejected;

    AddDomainEvent(
        new TechnicianVerificationRejectedDomainEvent(
            Id,
            UserId));

    return Result.Success;
}

public Result<Success> SubmitForVerification()
{
    if (VerificationStatus == VerificationStatus.Verified)
        return TechnicianProfileErrors.AlreadyVerified;

    if (VerificationStatus == VerificationStatus.Pending)
        return TechnicianProfileErrors.VerificationAlreadyPending;

    VerificationStatus = VerificationStatus.Pending;

    AddDomainEvent(
        new TechnicianVerificationSubmittedDomainEvent(
            Id,
            UserId));

    return Result.Success;
}

public Result<Success> UpdateBio(string? bio)
{
    bio = bio?.Trim();

    if (bio?.Length > 1000)
        return TechnicianProfileErrors.BioTooLong;

    if (Bio == bio)
        return TechnicianProfileErrors.SameBio;

    Bio = bio;

    return Result.Success;
}


public Result<Success> UpdateAvailability(
    TechnicianAvailability availability)
{
    if (Availability == availability)
        return TechnicianProfileErrors.SameAvailability;

    Availability = availability;

    AddDomainEvent(
        new TechnicianAvailabilityChangedDomainEvent(
            Id,
            availability));

    return Result.Success;
}


public Result<Success> ConfigureAvailability(
    TechnicianAvailabilitySettings availabilitySettings)
{
    if (availabilitySettings is null)
        return TechnicianAvailabilityErrors.AvailabilitySettingsRequired;

    if (AvailabilitySettings == availabilitySettings)
        return TechnicianProfileErrors.SameAvailabilitySettings;

    AvailabilitySettings = availabilitySettings;

    AddDomainEvent(
        new TechnicianAvailabilitySettingsUpdatedDomainEvent(
            Id,
            availabilitySettings.Status));

    return Result.Success;
}


public Result<Success> UpdateNationalId(
    string nationalIdImageKey)
{
    if (string.IsNullOrWhiteSpace(nationalIdImageKey))
        return TechnicianProfileErrors.NationalIdImageRequired;

    nationalIdImageKey = nationalIdImageKey.Trim();

    if (NationalIdImageKey == nationalIdImageKey)
        return TechnicianProfileErrors.SameNationalIdImage;

    NationalIdImageKey = nationalIdImageKey;

    return Result.Success;
}

public Result<Success> CompleteProfile()
{
    if (IsProfileCompleted)
        return TechnicianProfileErrors.ProfileAlreadyCompleted;

    if (string.IsNullOrWhiteSpace(Bio))
        return TechnicianProfileErrors.BioRequired;

    if (string.IsNullOrWhiteSpace(NationalIdImageKey))
        return TechnicianProfileErrors.NationalIdImageRequired;

    if (_services.Count == 0)
        return TechnicianProfileErrors.AtLeastOneServiceRequired;

    IsProfileCompleted = true;

    AddDomainEvent(new TechnicianProfileCompletedDomainEvent(Id));

    return Result.Success;
}

public Result<Success> AddService(
    TechnicianService service)
{
    if (_services.Any(x => x.ServiceCategoryId == service.ServiceCategoryId))
        return TechnicianProfileErrors.ServiceAlreadyAdded;

    _services.Add(service);

    AddDomainEvent(
        new TechnicianServiceAddedDomainEvent(
            Id,
            service.Id));

    return Result.Success;
}

public Result<Success> RemoveService(Guid serviceId)
{
    var service = _services.FirstOrDefault(x => x.Id == serviceId);

    if (service is null)
        return TechnicianProfileErrors.ServiceNotFound;

    _services.Remove(service);

    AddDomainEvent(
        new TechnicianServiceRemovedDomainEvent(
            Id,
            service.Id));

    return Result.Success;
}


public Result<Success> UpdateYearsOfExperience(
    int yearsOfExperience)
{
    if (yearsOfExperience < 0)
        return TechnicianProfileErrors.InvalidYearsOfExperience;

    if (YearsOfExperience == yearsOfExperience)
        return TechnicianProfileErrors.SameYearsOfExperience;

    YearsOfExperience = yearsOfExperience;

    return Result.Success;
}

public Result<Success> AddExperience(
    TechnicianExperience experience)
{
    if (experience is null)
        return TechnicianProfileErrors.ExperienceRequired;

    if (_experiences.Any(x => x.Id == experience.Id))
        return TechnicianProfileErrors.ExperienceAlreadyAdded;

    _experiences.Add(experience);

    AddDomainEvent(
        new TechnicianExperienceAddedDomainEvent(
            experience.Id,
            Id));

    return Result.Success;
}

public Result<Success> RemoveExperience(
    Guid experienceId)
{
    var experience = _experiences.FirstOrDefault(x => x.Id == experienceId);

    if (experience is null)
        return TechnicianProfileErrors.ExperienceNotFound;

    _experiences.Remove(experience);

    AddDomainEvent(
        new TechnicianExperienceRemovedDomainEvent(
            experience.Id,
            Id));

    return Result.Success;
}

public Result<Success> AddPortfolioItem(
    TechnicianPortfolioItem portfolioItem)
{
    if (portfolioItem is null)
        return TechnicianProfileErrors.PortfolioItemRequired;

    if (_portfolioItems.Any(x => x.Id == portfolioItem.Id))
        return TechnicianProfileErrors.PortfolioItemAlreadyAdded;

    _portfolioItems.Add(portfolioItem);

    AddDomainEvent(
        new TechnicianPortfolioItemCreatedDomainEvent(
            portfolioItem.Id,
            Id));

    return Result.Success;
}

public Result<Success> RemovePortfolioItem(
    Guid portfolioItemId)
{
    var portfolioItem = _portfolioItems.FirstOrDefault(x => x.Id == portfolioItemId);

    if (portfolioItem is null)
        return TechnicianProfileErrors.PortfolioItemNotFound;

    _portfolioItems.Remove(portfolioItem);

    AddDomainEvent(
        new TechnicianPortfolioItemRemovedDomainEvent(
            portfolioItem.Id,
            Id));

    return Result.Success;
}
}