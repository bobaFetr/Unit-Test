using System.Collections.Generic;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class SearchOptionsFactory
{
    public static SearchOptionsDto CreateExactNameSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "14.1-inch Laptop",
            ExpectedProducts = ProductFactory.CreateLaptopOnlySearchResults(),
            ExpectResults = true
        };
    }

    public static SearchOptionsDto CreatePartialNameSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "laptop",
            ExpectedProducts = ProductFactory.CreateLaptopSearchResults(),
            ExpectResults = true
        };
    }

    public static SearchOptionsDto CreateDifferentCasingSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "LAPTOP",
            ExpectedProducts = ProductFactory.CreateLaptopOnlySearchResults(),
            ExpectResults = true
        };
    }

    public static SearchOptionsDto CreateNonExistingSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "ProductThatDoesNotExist123",
            ExpectedProducts = ProductFactory.CreateEmptySearchResults(),
            ExpectResults = false
        };
    }

    public static IEnumerable<SearchOptionsDto> PositiveSearchCases()
    {
        yield return CreateExactNameSearch();
        yield return CreatePartialNameSearch();
        yield return CreateDifferentCasingSearch();
    }
}
