public sealed class Money : ValueObject
{
    public decimal Value { get; }

    public Currency Currency { get; }

    private Money(decimal value, Currency currency)
    {
        Value = value;
        Currency = currency;
    }

    public static Result<Money> Create(
        decimal value,
        Currency currency)
    {
        if (value <= 0)
            return MoneyErrors.InvalidAmount;

        return new Money(decimal.Round(value, 2), currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }

    public static Money? Sum(params Money?[] amounts)
    {
        var currency = amounts
            .FirstOrDefault(amount => amount is not null)?
            .Currency;

        if (currency is null)
            return null;

        var total = amounts
            .Where(amount => amount is not null)
            .Sum(amount => amount!.Value);

        return total <= 0
            ? null
            : new Money(total, currency.Value);
    }

    public static implicit operator decimal(Money money)
        => money.Value;

    public override string ToString()
        => $"{Value} {Currency}";
}