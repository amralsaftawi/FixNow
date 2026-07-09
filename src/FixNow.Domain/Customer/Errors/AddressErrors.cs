
public static class AddressErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Address.IdRequired",
            "Address id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "Address.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error LabelRequired =
        Error.Validation(
            "Address.LabelRequired",
            "Address label is required.");

    public static readonly Error CountryRequired =
        Error.Validation(
            "Address.CountryRequired",
            "Country is required.");

    public static readonly Error CityRequired =
        Error.Validation(
            "Address.CityRequired",
            "City is required.");

    public static readonly Error AreaRequired =
        Error.Validation(
            "Address.AreaRequired",
            "Area is required.");

    public static readonly Error StreetRequired =
        Error.Validation(
            "Address.StreetRequired",
            "Street is required.");

    public static readonly Error BuildingNumberRequired =
        Error.Validation(
            "Address.BuildingNumberRequired",
            "Building number is required.");

    public static readonly Error FullAddressRequired =
        Error.Validation(
            "Address.FullAddressRequired",
            "Full address is required.");

    public static readonly Error AlreadyDefault =
        Error.Conflict(
            "Address.AlreadyDefault",
            "This address is already the default address.");
}