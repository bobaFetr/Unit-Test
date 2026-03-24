namespace ECommerceTests.Infrastructure.DTOs;

public sealed record RegistrationUserDto
{
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string Title { get; init; } = "Mr";

    public string Day { get; init; } = "10";

    public string Month { get; init; } = "5";

    public string Year { get; init; } = "1995";

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Company { get; init; } = string.Empty;

    public string AddressLine1 { get; init; } = string.Empty;

    public string AddressLine2 { get; init; } = string.Empty;

    public string Country { get; init; } = "Canada";

    public string State { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string ZipCode { get; init; } = string.Empty;

    public string MobileNumber { get; init; } = string.Empty;
}
