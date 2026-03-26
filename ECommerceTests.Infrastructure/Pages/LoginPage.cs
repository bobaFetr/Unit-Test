using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class LoginPage : BasePage
{
    private readonly By _pageTitle = By.XPath("//div[contains(@class, 'login-page')]//h1[normalize-space()='Welcome, Please Sign In!']");
    private readonly By _returningCustomerTitle = By.XPath("//div[contains(@class, 'returning-wrapper')]//strong[normalize-space()='Returning Customer']");
    private readonly By _loginEmailInput = By.Id("Email");
    private readonly By _loginPasswordInput = By.Id("Password");
    private readonly By _loginButton = By.CssSelector("input.login-button");
    private readonly By _loginError = By.CssSelector(".message-error .validation-summary-errors span");
    private readonly By _registerButton = By.CssSelector("input.register-button");

    public LoginPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Login;

    public bool IsCurrentPage()
    {
        return IsElementDisplayed(_pageTitle) && IsElementDisplayed(_returningCustomerTitle);
    }

    public void Login(LoginUserDto user)
    {
        Type(_loginEmailInput, user.Email);
        Type(_loginPasswordInput, user.Password);
        Click(_loginButton);
    }

    public SignupPage StartSignup(RegistrationUserDto user)
    {
        Click(_registerButton);
        return new SignupPage(Driver);
    }

    public void EnterSignupEmail(string email)
    {
    }

    public void EnterSignupName(string name)
    {
    }

    public void SubmitSignup()
    {
    }

    public string GetLoginErrorText()
    {
        return GetText(_loginError);
    }

    public string GetExistingEmailErrorText()
    {
        return string.Empty;
    }

    public string GetSignupEmailValidationMessage()
    {
        return string.Empty;
    }

    public string GetSignupEmailValue()
    {
        return string.Empty;
    }
}
