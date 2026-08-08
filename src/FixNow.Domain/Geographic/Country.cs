
public sealed class Country
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    // Navigation

    public IReadOnlyCollection<City> Cities { get; private set; } = [];

#pragma warning disable CS8618
    private Country()
    {
    }
#pragma warning disable CS8618
    private Country(string name)
    {
        Name = name;
    }

    public static Result<Country> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CountryErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return CountryErrors.NameTooLong;

        return new Country(name);
    }
}
