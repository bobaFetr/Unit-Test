using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class ProductDetailsPage : BasePage
{
    private readonly By _productName = By.XPath("//div[contains(@class, 'product-information')]//h2");
    private readonly By _category = By.XPath("//div[contains(@class, 'product-information')]//p[contains(normalize-space(), 'Category:')]");
    private readonly By _brand = By.XPath("//div[contains(@class, 'product-information')]//p[contains(normalize-space(), 'Brand:')]");
    private readonly By _price = By.XPath("//div[contains(@class, 'product-information')]//span/span");
    private readonly By _quantityInput = By.XPath("//input[@id='quantity']");
    private readonly By _addToCartButton = By.XPath("//button[contains(@class, 'cart') and normalize-space()='Add to cart']");
    private readonly By _cartModal = By.XPath("//div[@id='cartModal']");
    private readonly By _viewCartModalLink = By.XPath("//div[@id='cartModal']//a[contains(@href, '/view_cart')]");

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
            Category = GetText(_category).Replace("Category:", string.Empty).Trim(),
            Brand = IsElementDisplayed(_brand, 2)
                ? GetText(_brand).Replace("Brand:", string.Empty).Trim()
                : string.Empty
        };
    }

    public void SetQuantity(int quantity)
    {
        Type(_quantityInput, quantity.ToString());
    }

    public void AddToCart()
    {
        Click(_addToCartButton);
        WaitHelper.UntilVisible(_cartModal);
    }

    public CartPage ViewCartFromModal()
    {
        Click(_viewCartModalLink);
        return new CartPage(Driver);
    }
}
