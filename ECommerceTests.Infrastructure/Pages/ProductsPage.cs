using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class ProductsPage : BasePage
{
    private readonly By _allProductsHeader = By.XPath("//h2[normalize-space()='All Products']");
    private readonly By _searchInput = By.XPath("//input[@id='search_product']");
    private readonly By _searchButton = By.XPath("//button[@id='submit_search']");
    private readonly By _searchedProductsHeader = By.XPath("//h2[normalize-space()='Searched Products']");
    private readonly By _productCards = By.XPath("//div[contains(@class, 'features_items')]//div[contains(@class, 'product-image-wrapper')]");
    private readonly By _continueShoppingButton = By.XPath("//button[normalize-space()='Continue Shopping']");
    private readonly By _viewCartModalLink = By.XPath("//div[@id='cartModal']//a[contains(@href, '/view_cart')]");
    private readonly By _cartModal = By.XPath("//div[@id='cartModal']");

    public ProductsPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Products;

    public bool IsCurrentPage()
    {
        return IsElementDisplayed(_allProductsHeader);
    }

    public IReadOnlyList<ProductDto> Search(SearchOptionsDto searchOptions)
    {
        Type(_searchInput, searchOptions.SearchTerm);
        Click(_searchButton);
        WaitForPageReady();
        return GetDisplayedProducts();
    }

    public IReadOnlyList<ProductDto> SubmitEmptySearch()
    {
        Type(_searchInput, string.Empty);
        Click(_searchButton);
        WaitForPageReady();
        return GetDisplayedProducts();
    }

    public string GetSearchInputValue()
    {
        return GetValue(_searchInput);
    }

    public bool IsSearchResultsHeadingVisible()
    {
        return IsElementDisplayed(_searchedProductsHeader);
    }

    public IReadOnlyList<ProductDto> GetDisplayedProducts()
    {
        if (!IsElementDisplayed(_productCards, 3))
        {
            return new List<ProductDto>();
        }

        return FindAllVisible(_productCards)
            .Select(MapProductCard)
            .Where(product => !string.IsNullOrWhiteSpace(product.Name))
            .Distinct()
            .ToList();
    }

    public void AddProductToCart(string productName)
    {
        ClickWithJavaScript(ProductAddToCartButton(productName));
        WaitHelper.UntilVisible(_cartModal);
    }

    public void ContinueShopping()
    {
        Click(_continueShoppingButton);
    }

    public CartPage ViewCartFromModal()
    {
        Click(_viewCartModalLink);
        return new CartPage(Driver);
    }

    public ProductDetailsPage ViewProduct(string productName)
    {
        Click(ProductViewLink(productName));
        return new ProductDetailsPage(Driver);
    }

    private ProductDto MapProductCard(IWebElement card)
    {
        var name = card.FindElement(By.XPath("(.//div[contains(@class, 'productinfo')]//p)[1]")).Text.Trim();
        var price = card.FindElement(By.XPath("(.//div[contains(@class, 'productinfo')]//h2)[1]")).Text.Trim();

        return new ProductDto
        {
            Name = name,
            Price = price
        };
    }

    private static By ProductAddToCartButton(string productName)
    {
        return By.XPath(
            $"(//div[contains(@class, 'product-image-wrapper')][.//p[normalize-space()={ToXPathLiteral(productName)}]]//a[contains(@class, 'add-to-cart')])[1]");
    }

    private static By ProductViewLink(string productName)
    {
        return By.XPath(
            $"//div[contains(@class, 'product-image-wrapper')][.//p[normalize-space()={ToXPathLiteral(productName)}]]//a[contains(@href, '/product_details/') and contains(normalize-space(), 'View Product')]");
    }
}
