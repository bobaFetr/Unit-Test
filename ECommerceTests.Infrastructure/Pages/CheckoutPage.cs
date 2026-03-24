using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class CheckoutPage : BasePage
{
    private readonly By _addressDetailsHeader = By.XPath("//h2[normalize-space()='Address Details']");
    private readonly By _reviewOrderHeader = By.XPath("//h2[normalize-space()='Review Your Order']");
    private readonly By _checkoutRows = By.XPath("//table[contains(@class, 'table-condensed')]//tr[starts-with(@id, 'product-')]");
    private readonly By _commentTextArea = By.XPath("//textarea[@name='message']");
    private readonly By _placeOrderButton = By.XPath("//a[normalize-space()='Place Order']");

    public CheckoutPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Urls.Checkout;

    public bool IsCurrentPage()
    {
        return IsElementDisplayed(_addressDetailsHeader) && IsElementDisplayed(_reviewOrderHeader);
    }

    public IReadOnlyList<ProductDto> GetOrderProducts()
    {
        if (!IsElementDisplayed(_checkoutRows, 3))
        {
            return new List<ProductDto>();
        }

        return FindAllVisible(_checkoutRows)
            .Select(MapCheckoutRow)
            .ToList();
    }

    public void EnterComment(string comment)
    {
        Type(_commentTextArea, comment);
    }

    public bool IsPlaceOrderVisible()
    {
        return IsElementDisplayed(_placeOrderButton);
    }

    private static ProductDto MapCheckoutRow(IWebElement row)
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
