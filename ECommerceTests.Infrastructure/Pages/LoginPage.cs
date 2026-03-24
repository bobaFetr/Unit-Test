using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class LoginPage : BasePage
{
    private readonly By _loginHeader = By.XPath("//h2[normalize-space()='Login to your account']");
    private readonly By _signupHeader = By.XPath("//h2[normalize-space()='New User Signup!']");
    private readonly By _loginEmailInput = By.XPath("//form[contains(@action, '/login')]//input[@data-qa='login-email' or @name='email']");
    private readonly By _loginPasswordInput = By.XPath("//form[contains(@action, '/login')]//input[@data-qa='login-password' or @name='password']");
    private readonly By _loginButton = By.XPath("//form[contains(@action, '/login')]//button[@data-qa='login-button' or normalize-space()='Login']");
    private readonly By _loginError = By.XPath("//form[contains(@action, '/login')]//p[contains(normalize-space(), 'Your email or password is incorrect')]");
    private readonly By _signupNameInput = By.XPath("//form[contains(@action, '/signup')]//input[@data-qa='signup-name' or @name='name']");
    private readonly By _signupEmailInput = By.XPath("//form[contains(@action, '/signup')]//input[@data-qa='signup-email' or @name='email']");
    private readonly By _signupButton = By.XPath("//form[contains(@action, '/signup')]//button[@data-qa='signup-button' or normalize-space()='Signup']");
    private readonly By _existingEmailError = By.XPath("//*[contains(normalize-space(), 'Email Address already exist')]");

    public LoginPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Login;

    public bool IsCurrentPage()
    {
        return IsElementDisplayed(_loginHeader) && IsElementDisplayed(_signupHeader);
    }

    public void Login(LoginUserDto user)
    {
        Type(_loginEmailInput, user.Email);
        Type(_loginPasswordInput, user.Password);
        Click(_loginButton);
    }

    public SignupPage StartSignup(RegistrationUserDto user)
    {
        Type(_signupNameInput, user.Name);
        Type(_signupEmailInput, user.Email);
        Click(_signupButton);
        return new SignupPage(Driver);
    }

    public void EnterSignupEmail(string email)
    {
        Type(_signupEmailInput, email);
    }

    public void EnterSignupName(string name)
    {
        Type(_signupNameInput, name);
    }

    public void SubmitSignup()
    {
        Click(_signupButton);
    }

    public string GetLoginErrorText()
    {
        return GetText(_loginError);
    }

    public string GetExistingEmailErrorText()
    {
        return GetText(_existingEmailError);
    }

    public string GetSignupEmailValidationMessage()
    {
        return GetValidationMessage(_signupEmailInput);
    }

    public string GetSignupEmailValue()
    {
        return GetValue(_signupEmailInput);
    }
}
