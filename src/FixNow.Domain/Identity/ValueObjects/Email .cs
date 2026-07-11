
public sealed class Email : ValueObject
{
    public string Value { get; }

#pragma warning disable CS8618
    private Email()
    {
    }
#pragma warning disable CS8618
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
              return EmailErrors.Required;

        value = value.Trim().ToLowerInvariant();

        if (value.Length > 320)
               return EmailErrors.TooLong;

        if (!IsValidEmail(value))
                return EmailErrors.InvalidFormat;

        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(Email email)
        => email.Value;
}