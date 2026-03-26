using System;
using System.Linq;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class ProductDetailsPage : BasePage
{
    private readonly By _productName = By.CssSelector(".product-name h1");
    private readonly By _price = By.CssSelector(".product-price [itemprop='price']");
    private readonly By _quantityInput = By.CssSelector(".add-to-cart .qty-input");
    private readonly By _addToCartButton = By.CssSelector(".add-to-cart .add-to-cart-button");
    private readonly By _cartQuantityLabel = By.CssSelector(".header-links .cart-qty");

    public ProductDetailsPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => string.Empty;

    public ProductDto GetProductDetails(string quantity = "1")
    {
        return new ProductDto
        {
            Name = GetText(_productName),
            Price = GetText(_price),
            Quantity = quantity,
            Category = GetCategoryPath(),
            Brand = string.Empty
        };
    }

    public void SetQuantity(int quantity)
    {
        Type(_quantityInput, quantity.ToString());
    }

    public void AddToCart()
    {
        var cartQuantityBefore = GetCartQuantity();
        Click(_addToCartButton);
        WaitHelper.Until(_ => GetCartQuantity() > cartQuantityBefore);
    }

    public CartPage ViewCartFromModal()
    {
        var cartPage = new CartPage(Driver);
        cartPage.Open();
        return cartPage;
    }

    private string GetCategoryPath()
    {
        var currentProductName = GetText(_productName);

        var categoryItems = Driver.FindElements(By.CssSelector(".breadcrumb li"))
            .Select(item => item.Text.Trim())
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .Select(text => text.Replace("/", string.Empty).Trim())
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .Where(text => !text.Equals("Home", StringComparison.OrdinalIgnoreCase))
            .Where(text => !text.Equals(currentProductName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return string.Join(" >> ", categoryItems);
    }

    private int GetCartQuantity()
    {
        var quantityText = GetText(_cartQuantityLabel);
        var digits = new string(quantityText.Where(char.IsDigit).ToArray());

        return int.TryParse(digits, out var quantity) ? quantity : 0;
    }
}
