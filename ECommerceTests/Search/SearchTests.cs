using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Base;
using ECommerceTests.Infrastructure.DTOs;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Search;

[TestFixture]
public sealed class SearchTests : BaseTest
{
    public static IEnumerable<TestCaseData> PositiveSearchCases()
    {
        yield return new TestCaseData(SearchOptionsFactory.CreateExactNameSearch())
            .SetName("Search_WithExactProductName_ShouldReturnExpectedProductDtos");
        yield return new TestCaseData(SearchOptionsFactory.CreatePartialNameSearch())
            .SetName("Search_WithPartialProductName_ShouldReturnExpectedProductDtos");
        yield return new TestCaseData(SearchOptionsFactory.CreateDifferentCasingSearch())
            .SetName("Search_WithDifferentCasing_ShouldReturnExpectedProductDtos");
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Products page.
    /// 2. Search for a product term provided by the test case source.
    /// 3. Compare the displayed product DTOs with the expected DTOs from the factory.
    /// Expected Result:
    /// The searched products section is shown and the displayed results match the expected DTO collection.
    /// </summary>
    [TestCaseSource(nameof(PositiveSearchCases))]
    public void Search_WithSupportedTerms_ShouldReturnExpectedProductDtos(SearchOptionsDto searchOptions)
    {
        var productsPage = HomePage.GoToProductsPage();
        var actualProducts = productsPage.Search(searchOptions);

        Assert.Multiple(() =>
        {
            Assert.That(
                productsPage.IsSearchResultsHeadingVisible(),
                Is.True,
                "The Products page should display the searched products heading after a successful search.");
            Assert.That(
                actualProducts,
                Is.EquivalentTo(searchOptions.ExpectedProducts),
                "The displayed search results DTO collection should match the expected DTO collection.");
            Assert.That(
                actualProducts.All(product => product.Name.Contains(searchOptions.SearchTerm, System.StringComparison.OrdinalIgnoreCase)),
                Is.True,
                "Each displayed search result should be relevant to the submitted search term.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Products page.
    /// 2. Submit the search form with an empty value.
    /// 3. Verify the products experience remains stable and usable.
    /// Expected Result:
    /// The page remains on Products, the search box stays empty, and at least one product card remains visible.
    /// </summary>
    [Test]
    public void Search_WithEmptyInput_ShouldKeepProductsPageUsable()
    {
        var productsPage = HomePage.GoToProductsPage();
        var actualProducts = productsPage.SubmitEmptySearch();

        Assert.Multiple(() =>
        {
            Assert.That(
                productsPage.IsCurrentPage() || productsPage.IsSearchResultsHeadingVisible(),
                Is.True,
                "Submitting an empty search should keep the user in a stable products browsing state.");
            Assert.That(
                productsPage.GetSearchInputValue(),
                Is.Empty,
                "The search input should remain empty after submitting an empty search.");
            Assert.That(
                actualProducts.Count,
                Is.GreaterThan(0),
                "Submitting an empty search should still leave visible products on the page.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Products page.
    /// 2. Search for a non-existing product term.
    /// 3. Verify the result collection is empty.
    /// Expected Result:
    /// The searched products section is displayed and no product DTOs are returned for the non-existing term.
    /// </summary>
    [TestCase("ProductThatDoesNotExist123")]
    public void Search_WithNonExistingProduct_ShouldReturnNoResults(string expectedTerm)
    {
        var searchOptions = SearchOptionsFactory.CreateNonExistingSearch();
        var productsPage = HomePage.GoToProductsPage();
        var actualProducts = productsPage.Search(searchOptions);

        Assert.Multiple(() =>
        {
            Assert.That(
                searchOptions.SearchTerm,
                Is.EqualTo(expectedTerm),
                "The factory DTO should provide the same non-existing search term used by the data-driven test.");
            Assert.That(
                productsPage.IsSearchResultsHeadingVisible(),
                Is.True,
                "The searched products heading should still be displayed when no products are found.");
            Assert.That(
                actualProducts,
                Is.EqualTo(searchOptions.ExpectedProducts),
                "A non-existing search should return an empty product DTO collection.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Products page and search for Blue Top.
    /// 2. Add the matching product to the cart and continue shopping.
    /// 3. Compare the search results before and after the interaction.
    /// Expected Result:
    /// The search results remain correct after adding a product to the cart and the search term stays in the input.
    /// </summary>
    [Test]
    public void SearchResults_ShouldRemainCorrectAfterCartInteraction()
    {
        var searchOptions = SearchOptionsFactory.CreateExactNameSearch();
        var productsPage = HomePage.GoToProductsPage();
        var searchResultsBeforeCart = productsPage.Search(searchOptions);

        productsPage.AddProductToCart(searchOptions.ExpectedProducts.Single().Name);
        productsPage.ContinueShopping();
        var searchResultsAfterCart = productsPage.GetDisplayedProducts();

        Assert.Multiple(() =>
        {
            Assert.That(
                searchResultsBeforeCart,
                Is.EqualTo(searchOptions.ExpectedProducts),
                "The initial search results should match the expected DTO collection before interacting with the cart.");
            Assert.That(
                searchResultsAfterCart,
                Is.EqualTo(searchOptions.ExpectedProducts),
                "The search results should remain unchanged after adding the searched product to the cart.");
            Assert.That(
                productsPage.GetSearchInputValue(),
                Is.EqualTo(searchOptions.SearchTerm),
                "The search input should preserve the original search term after cart interaction.");
        });
    }
}
