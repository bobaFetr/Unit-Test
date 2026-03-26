using System.Collections.Generic;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class ProductFactory
{
    public static ProductDto CreateLaptop(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "14.1-inch Laptop",
            Price = "1590.00",
            Quantity = quantity
        };
    }

    public static ProductDto CreateLaptopDetails(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "14.1-inch Laptop",
            Price = "1590.00",
            Quantity = quantity,
            Category = "COMPUTERS >> NOTEBOOKS",
            Brand = string.Empty
        };
    }

    public static IReadOnlyList<ProductDto> CreateLaptopOnlySearchResults()
    {
        return new[]
        {
            CreateLaptop()
        };
    }

    public static IReadOnlyList<ProductDto> CreateLaptopSearchResults()
    {
        return new[]
        {
            CreateLaptop()
        };
    }

    public static IReadOnlyList<ProductDto> CreateEmptySearchResults()
    {
        return new ProductDto[0];
    }
}
