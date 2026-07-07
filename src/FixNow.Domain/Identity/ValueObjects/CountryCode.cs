using System.Text.RegularExpressions;

public sealed class CountryCode : ValueObject
{
    public string Value { get; }

    private CountryCode()
    {
    }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static Result<CountryCode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return CountryCodeErrors.Required;

        value = value.Trim().ToUpperInvariant();

        if (value.Length != 2)
            return CountryCodeErrors.InvalidLength;

        if (!Regex.IsMatch(value, "^[A-Z]{2}$"))
            return CountryCodeErrors.InvalidFormat;


        return new CountryCode(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}