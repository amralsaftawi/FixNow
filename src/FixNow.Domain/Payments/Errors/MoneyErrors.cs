
public static class MoneyErrors
{
    public static readonly Error InvalidAmount =
        Error.Validation(
            "Money.InvalidAmount",
            "Amount must be greater than zero.");
}