public sealed class CustomerPaymentMethod : AuditableEntity
{
    public Guid CustomerProfileId { get; private set; }

    public PaymentMethod Type { get; private set; }

    public bool IsDefault { get; private set; }

    // Navigation

    public CustomerProfile CustomerProfile { get; private set; } = null!;

#pragma warning disable CS8618
    private CustomerPaymentMethod()
    {
    }
#pragma warning disable CS8618
    private CustomerPaymentMethod(
        Guid id,
        Guid customerProfileId,
        PaymentMethod type)
        : base(id)
    {
        CustomerProfileId = customerProfileId;
        Type = type;
        IsDefault = false;
    }

    public static Result<CustomerPaymentMethod> Create(
        Guid id,
        Guid customerProfileId,
        PaymentMethod type)
    {
        if (id == Guid.Empty)
            return CustomerPaymentMethodErrors.IdRequired;

        if (customerProfileId == Guid.Empty)
            return CustomerPaymentMethodErrors.CustomerProfileIdRequired;

        if (!Enum.IsDefined(type))
            return CustomerPaymentMethodErrors.TypeRequired;

        var paymentMethod = new CustomerPaymentMethod(
            id,
            customerProfileId,
            type);

        paymentMethod.AddDomainEvent(
            new CustomerPaymentMethodCreatedDomainEvent(
                paymentMethod.Id,
                paymentMethod.CustomerProfileId));

        return paymentMethod;
    }

    public Result<Success> SetAsDefault()
    {
        if (IsDefault)
            return CustomerPaymentMethodErrors.AlreadyDefault;

        IsDefault = true;

        AddDomainEvent(
            new CustomerPaymentMethodMarkedAsDefaultDomainEvent(
                Id,
                CustomerProfileId));

        return Result.Success;
    }

    public Result<Success> RemoveDefault()
    {
        if (!IsDefault)
            return Result.Success;

        IsDefault = false;

        return Result.Success;
    }
}
