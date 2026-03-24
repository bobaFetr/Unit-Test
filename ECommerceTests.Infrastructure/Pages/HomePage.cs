using ECommerceTests.Infrastructure.Constants;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class HomePage : BasePage
{
    private readonly By _signupLoginLink = By.XPath("//a[contains(@href, '/login') and normalize-space()='Signup / Login']");
    private readonly By _productsLink = By.XPath("//a[contains(@href, '/products') and normalize-space()='Products']");
    private readonly By _cartLink = By.XPath("//a[contains(@href, '/view_cart') and normalize-space()='Cart']");
    private readonly By _logoutLink = By.XPath("//a[contains(@href, '/logout') and normalize-space()='Logout']");
    private readonly By _loggedInUserLabel = By.XPath("//a[contains(normalize-space(), 'Logged in as')]");
    private readonly By _deleteAccountLink = By.XPath("//a[contains(@href, '/delete_account') and normalize-space()='Delete Account']");
    private readonly By _continueButton = By.XPath("//a[@data-qa='continue-button' or normalize-space()='Continue']");
    private readonly By _accountCreatedLabel = By.XPath("//*[contains(translate(normalize-space(), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), 'ACCOUNT CREATED!')]");
    private readonly By _accountDeletedLabel = By.XPath("//*[contains(translate(normalize-space(), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), 'ACCOUNT DELETED!')]");

    public HomePage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => string.Empty;

    public LoginPage GoToLoginPage()
    {
        Click(_signupLoginLink);
        return new LoginPage(Driver);
    }

    public ProductsPage GoToProductsPage()
    {
        Click(_productsLink);
        return new ProductsPage(Driver);
    }

    public CartPage GoToCartPage()
    {
        Click(_cartLink);
        return new CartPage(Driver);
    }

    public bool IsLoggedIn()
    {
        return IsElementDisplayed(_loggedInUserLabel);
    }

    public bool IsLogoutVisible()
    {
        return IsElementDisplayed(_logoutLink);
    }

    public string GetLoggedInUsername()
    {
        return GetText(_loggedInUserLabel).Replace(Messages.LoggedInAs, string.Empty).Trim();
    }

    public bool IsAccountCreatedVisible()
    {
        return IsElementDisplayed(_accountCreatedLabel);
    }

    public bool IsAccountDeletedVisible()
    {
        return IsElementDisplayed(_accountDeletedLabel);
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
        Click(_deleteAccountLink);
        WaitForPageReady();
    }

    public bool CanDeleteCurrentAccount()
    {
        return IsElementDisplayed(_deleteAccountLink, 2);
    }
}
