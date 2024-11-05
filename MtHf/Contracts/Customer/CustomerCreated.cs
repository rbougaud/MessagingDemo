namespace Contracts.Customer;

public record CustomerCreated(Guid Id, string FirstName, string LastName, string Mail, string? Address, string? Phone, string? Iban);
