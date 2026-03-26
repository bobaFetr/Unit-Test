using ECommerceTests.Base;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Registration;

[TestFixture]
[CancelAfter(20000)]
public sealed class RegistrationTests : BaseTest
{
    /// <summary>
    /// Steps:
    /// 1. Open the login page and move into the register flow for a factory-generated unique user.
    /// 2. Complete the register form and submit it.
    /// 3. Continue to the home page and verify the authenticated state.
    /// Expected Result:
    /// A new account is created successfully, the registration-complete message is shown, and the user is authenticated with the expected account email.
    /// </summary>
    [Test]
    public void Registration_WithUniqueEmail_ShouldCreateAccountSuccessfully()
    {
        var newUser = RegistrationUserFactory.CreateUniqueUser();
        var loginPage = HomePage.GoToLoginPage();
        var signupPage = loginPage.StartSignup(newUser);

        Assert.That(
            signupPage.IsEnterAccountInformationVisible(),
            Is.True,
            "The Register page should open after selecting the new-customer flow.");

        signupPage.CreateAccount(newUser);

        Assert.That(
            HomePage.IsAccountCreatedVisible(),
            Is.True,
            "A successful registration should display an account created confirmation message.");

        HomePage.ContinueAfterAccountAction();

        Assert.Multiple(() =>
        {
            Assert.That(
                HomePage.IsLoggedIn(),
                Is.True,
                "The newly registered user should already be authenticated on the registration result page.");
            Assert.That(
                HomePage.GetLoggedInUsername(),
                Is.EqualTo(newUser.Email),
                "The account link should show the new user's email after registration succeeds.");
        });

        Assert.That(
            HomePage.IsLogoutVisible(),
            Is.True,
            "The logout option should be available after registration completes.");
    }

    /// <summary>
    /// Steps:
    /// 1. Register a unique user once and log out.
    /// 2. Attempt to register the same email again.
    /// 3. Verify the duplicate email warning is shown.
    /// Expected Result:
    /// Registration is blocked and the application displays the expected existing email validation message.
    /// </summary>
    [Test]
    public void Registration_WithExistingEmail_ShouldShowDuplicateEmailMessage()
    {
        var existingUser = RegistrationUserFactory.CreateUniqueUser();
        var firstSignupPage = HomePage.GoToLoginPage().StartSignup(existingUser);
        firstSignupPage.CreateAccount(existingUser);
        Assert.That(
            HomePage.IsAccountCreatedVisible(),
            Is.True,
            "The precondition account should be created before the duplicate-email scenario is exercised.");

        HomePage.ContinueAfterAccountAction();
        HomePage.Logout();

        var duplicateSignupPage = HomePage.GoToLoginPage().StartSignup(existingUser);
        duplicateSignupPage.CreateAccount(existingUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                duplicateSignupPage.GetExistingEmailErrorText(),
                Is.EqualTo(Messages.ExistingEmail),
                "The registration form should display the expected duplicate-email validation message.");
            Assert.That(
                duplicateSignupPage.IsEnterAccountInformationVisible(),
                Is.True,
                "The user should remain on the Register page when the email already exists.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the register page from the login flow.
    /// 2. Fill the required registration fields with an invalid email format.
    /// 3. Submit the form and read the email validation message.
    /// Expected Result:
    /// The invalid email is rejected by the form, the user stays on the register page, and a validation message is available.
    /// </summary>
    [Test]
    public void Registration_WithInvalidEmailFormat_ShouldStayOnSignupPage()
    {
        var invalidUser = RegistrationUserFactory.CreateInvalidEmailUser();
        var signupPage = HomePage.GoToLoginPage().StartSignup(invalidUser);
        signupPage.CreateAccount(invalidUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                signupPage.GetSignupEmailValidationMessage(),
                Is.Not.Empty,
                "The browser should provide a validation message when the registration email format is invalid.");
            Assert.That(
                signupPage.IsEnterAccountInformationVisible(),
                Is.True,
                "The user should remain on the Register page when the registration data is invalid.");
            Assert.That(
                signupPage.GetSignupEmailValue(),
                Is.EqualTo(invalidUser.Email),
                "The invalid email value should remain in the email field so the user can correct it.");
        });
    }
}
