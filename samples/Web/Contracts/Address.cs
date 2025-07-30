namespace Web.Contracts
{
    public sealed record Address(
        int Id,
        string Line1,
        string? Line2,
        string City,
        string State,
        string PostalCode,
        string CountryCode);
}
