
public sealed class CustomerProfile : AuditableEntity
{
    public Guid UserId { get; private set; }

    public DateTimeOffset RegisteredAt { get; private set; }

    private readonly List<Address> _addresses = [];

    public IReadOnlyCollection<Address> Addresses =>
        _addresses.AsReadOnly();

    // Navigation

    public User User { get; private set; } = null!;

    private CustomerProfile()
    {
    }

    private CustomerProfile(
        Guid id,
        Guid userId)
        : base(id)
    {
        UserId = userId;
        RegisteredAt = DateTimeOffset.UtcNow;
    }

    public static Result<CustomerProfile> Create(
        Guid id,
        Guid userId)
    {
        if (id == Guid.Empty)
            return CustomerProfileErrors.IdRequired;

        if (userId == Guid.Empty)
            return CustomerProfileErrors.UserIdRequired;

        var profile = new CustomerProfile(
            id,
            userId);

        profile.AddDomainEvent(
            new CustomerProfileCreatedDomainEvent(
                profile.Id,
                profile.UserId));

        return profile;
    }

    public Result<Success> AddAddress(Address address)
    {
        if (address is null)
            return CustomerProfileErrors.AddressRequired;

        if (_addresses.Any(a => a.Id == address.Id))
            return CustomerProfileErrors.AddressAlreadyExists;

        _addresses.Add(address);

        AddDomainEvent(
            new CustomerAddressAddedDomainEvent(
                Id,
                address.Id));

        return Result.Success;
    }

    public Result<Success> RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            return CustomerProfileErrors.AddressNotFound;

        _addresses.Remove(address);

        AddDomainEvent(
            new CustomerAddressRemovedDomainEvent(
                Id,
                addressId));

        return Result.Success;
    }

    public Result<Success> SetDefaultAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            return CustomerProfileErrors.AddressNotFound;

        foreach (var item in _addresses.Where(a => a.IsDefault))
        {
            item.RemoveDefault();
        }

        address.SetAsDefault();

        AddDomainEvent(
            new CustomerDefaultAddressChangedDomainEvent(
                Id,
                addressId));

        return Result.Success;
    }
}