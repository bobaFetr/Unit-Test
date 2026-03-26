using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class ProductsPage : BasePage
{
    private readonly By _featuredProductsHeader = By.XPath("//div[contains(@class, 'home-page-product-grid')]//strong[normalize-space()='Featured products']");
    private readonly By _pageSearchHeader = By.XPath("//div[contains(@class, 'search-page')]//h1[normalize-space()='Search']");
    private readonly By _pageSearchInput = By.Id("Q");
    private readonly By _headerSearchInput = By.Id("small-searchterms");
    private readonly By _headerSearchButton = By.CssSelector("input.search-box-button");
    private readonly By _productCards = By.CssSelector(".product-item");
    private readonly By _searchResults = By.CssSelector(".search-results");
    private readonly By _notificationBar = By.Id("bar-notification");
    private readonly By _notificationCloseButton = By.CssSelector("#bar-notification .close");
    private readonly By _notificationCartLink = By.XPath("//*[@id='bar-notification']//a[contains(@href, '/cart')]");
    private readonly By _cartQuantityLabel = By.CssSelector(".header-links .cart-qty");

    public ProductsPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Products;

    public override void Open()
    {
        base.Open();
        DismissConsentIfPresent();
    }

    public bool IsCurrentPage()
    {
        return IsElementDisplayed(_featuredProductsHeader, 2) ||
               IsElementDisplayed(_pageSearchHeader, 2) ||
               IsElementDisplayed(_productCards, 2);
    }

    public IReadOnlyList<ProductDto> Search(SearchOptionsDto searchOptions)
    {
        if (!IsCurrentPage())
        {
            Open();
        }

        DismissConsentIfPresent();
        Type(_headerSearchInput, searchOptions.SearchTerm);
        Click(_headerSearchButton);
        WaitForPageReady();
        return GetDisplayedProducts();
    }

    public IReadOnlyList<ProductDto> SubmitEmptySearch()
    {
        if (!IsCurrentPage())
        {
            Open();
        }

        DismissConsentIfPresent();
        var searchInput = FindVisible(_headerSearchInput);
        JavaScriptHelper.ScrollIntoView(searchInput);
        searchInput.Clear();

        var searchButton = FindVisible(_headerSearchButton);
        JavaScriptHelper.ScrollIntoView(searchButton);
        searchButton.Click();

        TryAcceptAlert();
        WaitForPageReady();
        return GetDisplayedProducts();
    }

    public string GetSearchInputValue()
    {
        if (IsElementDisplayed(_pageSearchInput, 2))
        {
            return GetValue(_pageSearchInput);
        }

        if (IsElementDisplayed(_headerSearchInput, 2))
        {
            var value = GetValue(_headerSearchInput);
            return value == "Search store" ? string.Empty : value;
        }

        return string.Empty;
    }

    public bool IsSearchResultsHeadingVisible()
    {
        return IsElementDisplayed(_pageSearchHeader, 2) || IsElementDisplayed(_searchResults, 2);
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
        var cartQuantityBefore = GetCartQuantity();
        ClickWithJavaScript(ProductAddToCartButton(productName));
        WaitHelper.Until(_ => GetCartQuantity() > cartQuantityBefore || IsElementDisplayed(_notificationBar, 2));
    }

    public void ContinueShopping()
    {
        if (IsElementDisplayed(_notificationCloseButton, 2))
        {
            Click(_notificationCloseButton);
        }
    }

    public CartPage ViewCartFromModal()
    {
        var cartPage = new CartPage(Driver);

        if (IsElementDisplayed(_notificationCartLink, 2))
        {
            Click(_notificationCartLink);
            return cartPage;
        }

        cartPage.Open();
        return cartPage;
    }

    public ProductDetailsPage ViewProduct(string productName)
    {
        Click(ProductViewLink(productName));
        return new ProductDetailsPage(Driver);
    }

    private ProductDto MapProductCard(IWebElement card)
    {
        var name = card.FindElement(By.CssSelector(".product-title a")).Text.Trim();
        var price = card.FindElement(By.CssSelector(".prices .price")).Text.Trim();

        return new ProductDto
        {
            Name = name,
            Price = price
        };
    }

    private int GetCartQuantity()
    {
        if (!IsElementDisplayed(_cartQuantityLabel, 2))
        {
            return 0;
        }

        var quantityText = GetText(_cartQuantityLabel);
        var digits = new string(quantityText.Where(char.IsDigit).ToArray());

        return int.TryParse(digits, out var quantity) ? quantity : 0;
    }

    private void TryAcceptAlert()
    {
        try
        {
            Driver.SwitchTo().Alert().Accept();
        }
        catch (NoAlertPresentException)
        {
        }
    }

    private static By ProductAddToCartButton(string productName)
    {
        return By.XPath(
            $"(//div[contains(@class, 'product-item')][.//h2[contains(@class, 'product-title')]//a[normalize-space()={ToXPathLiteral(productName)}]]//input[contains(@class, 'product-box-add-to-cart-button')])[1]");
    }

    private static By ProductViewLink(string productName)
    {
        return By.XPath(
            $"//div[contains(@class, 'product-item')][.//h2[contains(@class, 'product-title')]//a[normalize-space()={ToXPathLiteral(productName)}]]//h2[contains(@class, 'product-title')]//a");
    }
}
