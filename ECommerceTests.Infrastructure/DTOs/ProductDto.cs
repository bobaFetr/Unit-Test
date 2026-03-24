namespace ECommerceTests.Infrastructure.DTOs;

public sealed record ProductDto
{
    public string Name { get; init; } = string.Empty;

    public string Price { get; init; } = string.Empty;

    public string Quantity { get; init; } = "1";

    public string Category { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;
}
