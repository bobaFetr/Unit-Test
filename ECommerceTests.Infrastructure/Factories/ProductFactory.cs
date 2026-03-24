using System.Collections.Generic;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class ProductFactory
{
    public static ProductDto CreateBlueTop(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "Blue Top",
            Price = "Rs. 500",
            Quantity = quantity
        };
    }

    public static ProductDto CreateBlueTopDetails(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "Blue Top",
            Price = "Rs. 500",
            Quantity = quantity,
            Category = "Women > Tops",
            Brand = "Polo"
        };
    }

    public static ProductDto CreateMenTshirt(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "Men Tshirt",
            Price = "Rs. 400",
            Quantity = quantity
        };
    }

    public static ProductDto CreateWinterTop(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "Winter Top",
            Price = "Rs. 600",
            Quantity = quantity
        };
    }

    public static ProductDto CreateSummerWhiteTop(string quantity = "1")
    {
        return new ProductDto
        {
            Name = "Summer White Top",
            Price = "Rs. 400",
            Quantity = quantity
        };
    }

    public static IReadOnlyList<ProductDto> CreateBlueTopOnlySearchResults()
    {
        return new[]
        {
            CreateBlueTop()
        };
    }

    public static IReadOnlyList<ProductDto> CreateTopSearchResults()
    {
        return new[]
        {
            CreateBlueTop(),
            CreateWinterTop(),
            CreateSummerWhiteTop()
        };
    }

    public static IReadOnlyList<ProductDto> CreateEmptySearchResults()
    {
        return new ProductDto[0];
    }
}
