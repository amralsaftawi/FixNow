public sealed class CustomerRatingScore : ValueObject
{
    public int Value { get; }

    private CustomerRatingScore(int value)
    {
        Value = value;
    }

    public static Result<CustomerRatingScore> Create(int value)
    {
        if (value < 1 || value > 5)
            return CustomerRatingErrors.InvalidRating;

        return new CustomerRatingScore(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(CustomerRatingScore score)
        => score.Value;
}
