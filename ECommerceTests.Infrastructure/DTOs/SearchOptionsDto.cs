using System.Collections.Generic;

namespace ECommerceTests.Infrastructure.DTOs;

public sealed record SearchOptionsDto
{
    public string SearchTerm { get; init; } = string.Empty;

    public IReadOnlyList<ProductDto> ExpectedProducts { get; init; } = new List<ProductDto>();

    public bool ExpectResults { get; init; } = true;
}
