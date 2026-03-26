using System.Collections.Generic;
using System.Linq;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class CheckoutPage : BasePage
{
    private readonly By _checkoutSteps = By.CssSelector("#checkout-steps, .one-page-checkout, .checkout-page");
    private readonly By _billingAddressStep = By.CssSelector("#opc-billing, .billing-address");
    private readonly By _checkoutRows = By.CssSelector("table.cart tbody tr.cart-item-row");
    private readonly By _commentTextArea = By.CssSelector("textarea");
    private readonly By _placeOrderButton = By.CssSelector("#confirm-order-buttons-container input, .confirm-order-next-step-button");

    public CheckoutPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Urls.Checkout;

    public bool IsCurrentPage()
    {
        return Driver.Url.Contains(Urls.Checkout, System.StringComparison.OrdinalIgnoreCase) ||
               IsElementDisplayed(_checkoutSteps, 5);
    }

    public bool IsBillingAddressStepVisible()
    {
        return IsElementDisplayed(_billingAddressStep, 5);
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
        if (IsElementDisplayed(_commentTextArea, 2))
        {
            Type(_commentTextArea, comment);
        }
    }

    public bool IsPlaceOrderVisible()
    {
        return IsElementDisplayed(_placeOrderButton, 2);
    }

    private static ProductDto MapCheckoutRow(IWebElement row)
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
