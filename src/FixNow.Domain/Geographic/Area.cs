
public sealed class Area
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int CityId { get; private set; }

    // Navigation

    public City? City { get; private set; }

#pragma warning disable CS8618
    private Area()
    {
    }
#pragma warning disable CS8618
    private Area(
        int cityId,
        string name)
    {
        CityId = cityId;
        Name = name;
    }

    public static Result<Area> Create(
        int cityId,
        string? name)
    {
        if (cityId <= 0)
            return AreaErrors.CityRequired;

        if (string.IsNullOrWhiteSpace(name))
            return AreaErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return AreaErrors.NameTooLong;

        return new Area(
            cityId,
            name);
    }
}
