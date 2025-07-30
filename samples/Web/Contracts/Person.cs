namespace Web.Contracts
{
    public sealed record Person(
        int Id,
        string EmailAddress,
        string FirstName,
        string LastName,
        Address MailingAddress);
}
