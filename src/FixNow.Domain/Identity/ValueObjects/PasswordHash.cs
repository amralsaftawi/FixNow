
public sealed record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static Result<PasswordHash> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return PasswordHashErrors.Required;

        value = value.Trim();

        // Prevent storing plain text passwords
        if (value.Length < 40)
            return PasswordHashErrors.InvalidHash;

        return new PasswordHash(value);
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordHash hash)
        => hash.Value;
}