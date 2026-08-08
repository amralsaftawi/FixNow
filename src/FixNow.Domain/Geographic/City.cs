
public sealed class City
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int CountryId { get; private set; }

    // Navigation

    public Country? Country { get; private set; }

    public IReadOnlyCollection<Area> Areas { get; private set; } = [];

#pragma warning disable CS8618
    private City()
    {
    }
#pragma warning disable CS8618
    private City(
        int countryId,
        string name)
    {
        CountryId = countryId;
        Name = name;
    }

    public static Result<City> Create(
        int countryId,
        string? name)
    {
        if (countryId <= 0)
            return CityErrors.CountryRequired;

        if (string.IsNullOrWhiteSpace(name))
            return CityErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return CityErrors.NameTooLong;

        return new City(
            countryId,
            name);
    }
}
