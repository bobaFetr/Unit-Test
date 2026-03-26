using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class SignupPage : BasePage
{
    private readonly By _registerHeader = By.XPath("//div[contains(@class, 'registration-page')]//h1[normalize-space()='Register']");
    private readonly By _maleTitleRadio = By.Id("gender-male");
    private readonly By _femaleTitleRadio = By.Id("gender-female");
    private readonly By _firstNameInput = By.Id("FirstName");
    private readonly By _lastNameInput = By.Id("LastName");
    private readonly By _emailInput = By.Id("Email");
    private readonly By _emailValidationMessage = By.CssSelector("[data-valmsg-for='Email']");
    private readonly By _passwordInput = By.Id("Password");
    private readonly By _confirmPasswordInput = By.Id("ConfirmPassword");
    private readonly By _createAccountButton = By.Id("register-button");
    private readonly By _existingEmailError = By.CssSelector(".message-error .validation-summary-errors li");

    public SignupPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Signup;

    public bool IsEnterAccountInformationVisible()
    {
        return IsElementDisplayed(_registerHeader);
    }

    public string GetLockedEmail()
    {
        return GetValue(_emailInput);
    }

    public void CreateAccount(RegistrationUserDto user)
    {
        if (user.Title.Equals("Mrs", System.StringComparison.OrdinalIgnoreCase) ||
            user.Title.Equals("Ms", System.StringComparison.OrdinalIgnoreCase) ||
            user.Title.Equals("Female", System.StringComparison.OrdinalIgnoreCase))
        {
            ClickWithJavaScript(_femaleTitleRadio);
        }
        else
        {
            ClickWithJavaScript(_maleTitleRadio);
        }

        Type(_firstNameInput, user.FirstName);
        Type(_lastNameInput, user.LastName);
        Type(_emailInput, user.Email);
        Type(_passwordInput, user.Password);
        Type(_confirmPasswordInput, user.Password);
        Click(_createAccountButton);
    }

    public string GetExistingEmailErrorText()
    {
        return GetText(_existingEmailError);
    }

    public string GetSignupEmailValidationMessage()
    {
        var browserValidationMessage = GetValidationMessage(_emailInput);
        if (!string.IsNullOrWhiteSpace(browserValidationMessage))
        {
            return browserValidationMessage;
        }

        return IsElementDisplayed(_emailValidationMessage, 2)
            ? GetText(_emailValidationMessage)
            : string.Empty;
    }

    public string GetSignupEmailValue()
    {
        return GetValue(_emailInput);
    }
}
