using ECommerceTests.Infrastructure.DTOs;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Pages;

public sealed class SignupPage : BasePage
{
    private readonly By _enterAccountInformationHeader = By.XPath("//b[normalize-space()='Enter Account Information']");
    private readonly By _mrTitleRadio = By.XPath("//input[@id='id_gender1']");
    private readonly By _mrsTitleRadio = By.XPath("//input[@id='id_gender2']");
    private readonly By _nameInput = By.XPath("//input[@id='name']");
    private readonly By _emailInput = By.XPath("//input[@id='email']");
    private readonly By _passwordInput = By.XPath("//input[@id='password']");
    private readonly By _daysDropdown = By.XPath("//select[@id='days']");
    private readonly By _monthsDropdown = By.XPath("//select[@id='months']");
    private readonly By _yearsDropdown = By.XPath("//select[@id='years']");
    private readonly By _newsletterCheckbox = By.XPath("//input[@id='newsletter']");
    private readonly By _offersCheckbox = By.XPath("//input[@id='optin']");
    private readonly By _firstNameInput = By.XPath("//input[@id='first_name']");
    private readonly By _lastNameInput = By.XPath("//input[@id='last_name']");
    private readonly By _companyInput = By.XPath("//input[@id='company']");
    private readonly By _addressLine1Input = By.XPath("//input[@id='address1']");
    private readonly By _addressLine2Input = By.XPath("//input[@id='address2']");
    private readonly By _countryDropdown = By.XPath("//select[@id='country']");
    private readonly By _stateInput = By.XPath("//input[@id='state']");
    private readonly By _cityInput = By.XPath("//input[@id='city']");
    private readonly By _zipCodeInput = By.XPath("//input[@id='zipcode']");
    private readonly By _mobileNumberInput = By.XPath("//input[@id='mobile_number']");
    private readonly By _createAccountButton = By.XPath("//button[@data-qa='create-account' or normalize-space()='Create Account']");

    public SignupPage(IWebDriver driver)
        : base(driver)
    {
    }

    public override string RelativeUrl => Constants.Urls.Signup;

    public bool IsEnterAccountInformationVisible()
    {
        return IsElementDisplayed(_enterAccountInformationHeader);
    }

    public string GetLockedEmail()
    {
        return GetValue(_emailInput);
    }

    public void CreateAccount(RegistrationUserDto user)
    {
        if (user.Title.Equals("Mrs", System.StringComparison.OrdinalIgnoreCase))
        {
            ClickWithJavaScript(_mrsTitleRadio);
        }
        else
        {
            ClickWithJavaScript(_mrTitleRadio);
        }

        Type(_passwordInput, user.Password);
        SelectByText(_daysDropdown, user.Day);
        SelectByText(_monthsDropdown, user.Month);
        SelectByText(_yearsDropdown, user.Year);
        ClickWithJavaScript(_newsletterCheckbox);
        ClickWithJavaScript(_offersCheckbox);
        Type(_firstNameInput, user.FirstName);
        Type(_lastNameInput, user.LastName);
        Type(_companyInput, user.Company);
        Type(_addressLine1Input, user.AddressLine1);
        Type(_addressLine2Input, user.AddressLine2);
        SelectByText(_countryDropdown, user.Country);
        Type(_stateInput, user.State);
        Type(_cityInput, user.City);
        Type(_zipCodeInput, user.ZipCode);
        Type(_mobileNumberInput, user.MobileNumber);
        Click(_createAccountButton);
    }
}
