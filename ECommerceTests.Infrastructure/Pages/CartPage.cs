using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class CartPage : BasePage
{
    private readonly By _cartRows = By.XPath("//table[@id='cart_info_table']//tr[starts-with(@id, 'product-')]");
    private readonly By _proceedToCheckoutButton = By.XPath("//a[contains(@class, 'check_out') and normalize-space()='Proceed To Checkout']");
    private readonly By _emptyCartMessage = By.XPath("//*[contains(normalize-space(), 'Cart is empty')]");

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
        Click(RemoveButton(productName));
        WaitHelper.Until(driver => driver.FindElements(_cartRows).All(
            row => !row.Text.Contains(productName, System.StringComparison.OrdinalIgnoreCase)));
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
        Click(_proceedToCheckoutButton);
        return new CheckoutPage(Driver);
    }

    private static By RemoveButton(string productName)
    {
        return By.XPath(
            $"//tr[starts-with(@id, 'product-')][.//td[contains(@class, 'cart_description')]//a[normalize-space()={ToXPathLiteral(productName)}]]//a[contains(@class, 'cart_quantity_delete')]");
    }

    private static ProductDto MapCartRow(IWebElement row)
    {
        var name = row.FindElement(By.XPath(".//td[contains(@class, 'cart_description')]//a")).Text.Trim();
        var price = row.FindElement(By.XPath(".//td[contains(@class, 'cart_price')]//p")).Text.Trim();
        var quantity = row.FindElement(By.XPath(".//td[contains(@class, 'cart_quantity')]//*[self::button or self::input]")).Text.Trim();

        return new ProductDto
        {
            Name = name,
            Price = price,
            Quantity = quantity
        };
    }
}
