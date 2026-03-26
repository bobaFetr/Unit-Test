using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class CartPage : BasePage
{
    private readonly By _cartRows = By.CssSelector("table.cart tbody tr.cart-item-row");
    private readonly By _updateCartButton = By.CssSelector("input.update-cart-button");
    private readonly By _proceedToCheckoutButton = By.Id("checkout");
    private readonly By _termsOfServiceCheckbox = By.Id("termsofservice");
    private readonly By _emptyCartMessage = By.XPath("//*[contains(normalize-space(), 'Your Shopping Cart is empty')]");

    public CartPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Cart;

    public IReadOnlyList<ProductDto> GetProducts()
    {
        if (!IsElementDisplayed(_cartRows, 3))
        {
            return new List<ProductDto>();
        }

        return FindAllVisible(_cartRows)
            .Select(MapCartRow)
            .ToList();
    }

    public void RemoveProduct(string productName)
    {
        Click(RemoveCheckbox(productName));
        Click(_updateCartButton);
        WaitHelper.Until(driver => driver.FindElements(_cartRows).All(
            row => !row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase)));
    }

    public bool IsEmpty()
    {
        return !Driver.FindElements(_cartRows).Any() || IsElementDisplayed(_emptyCartMessage, 2);
    }

    public void ClearCart()
    {
        while (Driver.FindElements(_cartRows).Any())
        {
            var firstProductName = GetProducts().First().Name;
            RemoveProduct(firstProductName);
        }
    }

    public CheckoutPage ProceedToCheckout()
    {
        if (IsElementDisplayed(_termsOfServiceCheckbox, 2))
        {
            var termsCheckbox = FindVisible(_termsOfServiceCheckbox);
            if (!termsCheckbox.Selected)
            {
                termsCheckbox.Click();
            }
        }

        Click(_proceedToCheckoutButton);
        return new CheckoutPage(Driver);
    }

    private static By RemoveCheckbox(string productName)
    {
        return By.XPath(
            $"//tr[contains(@class, 'cart-item-row')][.//a[contains(@class, 'product-name') and normalize-space()={ToXPathLiteral(productName)}]]//td[contains(@class, 'remove-from-cart')]//input[@type='checkbox']");
    }

    private static ProductDto MapCartRow(IWebElement row)
    {
        var name = row.FindElement(By.CssSelector(".product-name")).Text.Trim();
        var price = row.FindElement(By.CssSelector(".product-unit-price")).Text.Trim();
        var quantity = row.FindElement(By.CssSelector(".qty-input")).GetAttribute("value")?.Trim() ?? string.Empty;

        return new ProductDto
        {
            Name = name,
            Price = price,
            Quantity = quantity
        };
    }
}
