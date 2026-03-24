using ECommerceTests.Base;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Registration;

[TestFixture]
public sealed class RegistrationTests : BaseTest
{
    /// <summary>
    /// Steps:
    /// 1. Open the Signup / Login page.
    /// 2. Start signup with a factory-generated unique user.
    /// 3. Complete the account information form and create the account.
    /// 4. Continue to the home page, verify the authenticated state, and delete the newly created account.
    /// Expected Result:
    /// A new account is created successfully, the user is logged in with the expected name, and the account can be deleted cleanly.
    /// </summary>
    [Test]
    public void Registration_WithUniqueEmail_ShouldCreateAccountSuccessfully()
    {
        var newUser = RegistrationUserFactory.CreateUniqueUser();
        var loginPage = HomePage.GoToLoginPage();
        var signupPage = loginPage.StartSignup(newUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                signupPage.IsEnterAccountInformationVisible(),
                Is.True,
                "The Enter Account Information page should open after a valid signup request.");
            Assert.That(
                signupPage.GetLockedEmail(),
                Is.EqualTo(newUser.Email),
                "The email on the account information page should match the generated registration DTO.");
        });

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
                "The newly registered user should be automatically logged in after account creation.");
            Assert.That(
                HomePage.GetLoggedInUsername(),
                Is.EqualTo(newUser.Name),
                "The logged-in user label should match the name from the registration DTO.");
        });

        HomePage.DeleteCurrentAccount();

        Assert.That(
            HomePage.IsAccountDeletedVisible(),
            Is.True,
            "The newly created account should be deleted successfully during cleanup.");

        HomePage.ContinueAfterAccountAction();
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Signup / Login page.
    /// 2. Attempt registration with an email that already exists in the system.
    /// 3. Verify the duplicate email warning is shown.
    /// Expected Result:
    /// Registration is blocked and the application displays the expected existing email validation message.
    /// </summary>
    [Test]
    public void Registration_WithExistingEmail_ShouldShowDuplicateEmailMessage()
    {
        EnsureExistingLoginCredentialsAreConfigured();
        var existingUser = RegistrationUserFactory.CreateExistingEmailUser();
        var loginPage = HomePage.GoToLoginPage();

        loginPage.StartSignup(existingUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                loginPage.GetExistingEmailErrorText(),
                Is.EqualTo(Messages.ExistingEmail),
                "The signup form should display the expected duplicate email validation message.");
            Assert.That(
                loginPage.IsCurrentPage(),
                Is.True,
                "The user should remain on the Signup / Login page when the email already exists.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Signup / Login page.
    /// 2. Enter a valid name and an invalid email format in the signup form.
    /// 3. Submit the form and read the browser validation message.
    /// Expected Result:
    /// The invalid email is rejected by the form, the user stays on the signup page, and a validation message is available.
    /// </summary>
    [Test]
    public void Registration_WithInvalidEmailFormat_ShouldStayOnSignupPage()
    {
        var invalidUser = RegistrationUserFactory.CreateInvalidEmailUser();
        var loginPage = HomePage.GoToLoginPage();

        loginPage.EnterSignupName(invalidUser.Name);
        loginPage.EnterSignupEmail(invalidUser.Email);
        loginPage.SubmitSignup();

        Assert.Multiple(() =>
        {
            Assert.That(
                loginPage.GetSignupEmailValidationMessage(),
                Is.Not.Empty,
                "The browser should provide a validation message when the signup email format is invalid.");
            Assert.That(
                loginPage.IsCurrentPage(),
                Is.True,
                "The user should remain on the Signup / Login page when the signup data is invalid.");
            Assert.That(
                loginPage.GetSignupEmailValue(),
                Is.EqualTo(invalidUser.Email),
                "The invalid email value should remain in the email field so the user can correct it.");
        });
    }
}
