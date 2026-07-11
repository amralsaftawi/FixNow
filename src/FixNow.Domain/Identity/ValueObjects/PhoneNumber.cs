using System.Text.RegularExpressions;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

#pragma warning disable CS8618
    private PhoneNumber()
    {
    }
#pragma warning disable CS8618
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return PhoneNumberErrors.Required;

        value = value.Trim().Replace(" ", "");

        if (!value.StartsWith('+'))
            return PhoneNumberErrors.MustStartWithPlus;

        if (!Regex.IsMatch(value, @"^\+[1-9]\d{7,14}$"))
            return PhoneNumberErrors.InvalidFormat;

        return new PhoneNumber(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(PhoneNumber phone)
        => phone.Value;
}
