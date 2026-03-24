using System.Collections.Generic;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class SearchOptionsFactory
{
    public static SearchOptionsDto CreateExactNameSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "Blue Top",
            ExpectedProducts = ProductFactory.CreateBlueTopOnlySearchResults(),
            ExpectResults = true
        };
    }

    public static SearchOptionsDto CreatePartialNameSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "Top",
            ExpectedProducts = ProductFactory.CreateTopSearchResults(),
            ExpectResults = true
        };
    }

    public static SearchOptionsDto CreateDifferentCasingSearch()
    {
        return new SearchOptionsDto
        {
            SearchTerm = "blue top",
            ExpectedProducts = ProductFactory.CreateBlueTopOnlySearchResults(),
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
