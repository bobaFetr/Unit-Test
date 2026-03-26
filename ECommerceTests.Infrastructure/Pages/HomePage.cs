using ECommerceTests.Infrastructure.Constants;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class HomePage : BasePage
{
    private readonly By _loginLink = By.XPath("//a[contains(@href, '/login') and normalize-space()='Log in']");
    private readonly By _cartLink = By.XPath("//a[contains(@href, '/cart') and contains(@class, 'ico-cart')]");
    private readonly By _logoutLink = By.XPath("//a[contains(@href, '/logout') and normalize-space()='Log out']");
    private readonly By _accountLink = By.XPath("//div[contains(@class, 'header-links')]//a[contains(@class, 'account')]");
    private readonly By _continueButton = By.XPath("//input[contains(@class, 'register-continue-button') or @value='Continue']");
    private readonly By _accountCreatedLabel = By.XPath("//div[contains(@class, 'registration-result-page')]//div[contains(@class, 'result') and contains(normalize-space(), 'Your registration completed')]");

    public HomePage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Urls.Home;

    public override void Open()
    {
        base.Open();
        DismissConsentIfPresent();
    }

    public LoginPage GoToLoginPage()
    {
        Click(_loginLink);
        return new LoginPage(Driver);
    }

    public ProductsPage GoToProductsPage()
    {
        var productsPage = new ProductsPage(Driver);
        productsPage.Open();
        return productsPage;
    }

    public CartPage GoToCartPage()
    {
        Click(_cartLink);
        return new CartPage(Driver);
    }

    public bool IsLoggedIn()
    {
        return IsElementDisplayed(_accountLink, 2) || IsElementDisplayed(_logoutLink, 2);
    }

    public bool IsLogoutVisible()
    {
        return IsElementDisplayed(_logoutLink);
    }

    public string GetLoggedInUsername()
    {
        if (!IsElementDisplayed(_accountLink, 2))
        {
            return string.Empty;
        }

        var accountText = GetText(_accountLink).Trim();

        return string.IsNullOrEmpty(Messages.LoggedInAs)
            ? accountText
            : accountText.Replace(Messages.LoggedInAs, string.Empty).Trim();
    }

    public bool IsAccountCreatedVisible()
    {
        return IsElementDisplayed(_accountCreatedLabel);
    }

    public bool IsAccountDeletedVisible()
    {
        return false;
    }

    public void ContinueAfterAccountAction()
    {
        if (IsElementDisplayed(_continueButton, 2))
        {
            Click(_continueButton);
        }
    }

    public void DeleteCurrentAccount()
    {
    }

    public void Logout()
    {
        if (IsElementDisplayed(_logoutLink, 2))
        {
            Click(_logoutLink);
        }
    }

    public bool CanDeleteCurrentAccount()
    {
        return false;
    }
}
