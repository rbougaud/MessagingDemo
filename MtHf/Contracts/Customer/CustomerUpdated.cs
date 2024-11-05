namespace Contracts.Customer;

public record CustomerUpdated(Guid Id, string FirstName, string LastName, string Mail, string? Address, string? Phone, string? Iban);
