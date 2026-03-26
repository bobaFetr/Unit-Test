using ECommerceTests.Base;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Login;

[TestFixture]
[CancelAfter(20000)]
public sealed class LoginTests : BaseTest
{
    /// <summary>
    /// Steps:
    /// 1. Register a unique user through the new-customer flow.
    /// 2. Log out and return to the login page.
    /// 3. Log in with the DTO built from the registered user and verify the authenticated state.
    /// Expected Result:
    /// The user is logged in successfully, the logout option is visible, and the account link shows the expected email.
    /// </summary>
    [Test]
    public void ValidLogin_WithExistingCredentials_ShouldLogInSuccessfully()
    {
        var registrationUser = RegistrationUserFactory.CreateUniqueUser();
        var registerPage = HomePage.GoToLoginPage().StartSignup(registrationUser);
        registerPage.CreateAccount(registrationUser);

        Assert.That(
            HomePage.IsAccountCreatedVisible(),
            Is.True,
            "The precondition account should be created successfully before validating the login flow.");

        HomePage.ContinueAfterAccountAction();
        HomePage.Logout();

        var validUser = LoginUserFactory.CreateFromRegistration(registrationUser);
        var loginPage = HomePage.GoToLoginPage();

        Assert.That(
            loginPage.IsCurrentPage(),
            Is.True,
            "The Demo Web Shop login page should display the expected sign-in sections before login is attempted.");

        loginPage.Login(validUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                HomePage.IsLoggedIn(),
                Is.True,
                "The application should display the logged-in user banner after successful authentication.");
            Assert.That(
                HomePage.IsLogoutVisible(),
                Is.True,
                "The logout navigation link should be visible after a successful login.");
            Assert.That(
                HomePage.GetLoggedInUsername(),
                Is.EqualTo(registrationUser.Email),
                "The account link should contain the authenticated user's email after a successful login.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Register a unique user and log out.
    /// 2. Attempt to log in with the correct email and an intentionally wrong password.
    /// 3. Verify the login attempt is rejected with a meaningful error.
    /// Expected Result:
    /// The user remains unauthenticated and the incorrect credentials error message is displayed.
    /// </summary>
    [Test]
    public void InvalidLogin_WithWrongPassword_ShouldShowValidationMessage()
    {
        var registrationUser = RegistrationUserFactory.CreateUniqueUser();
        var registerPage = HomePage.GoToLoginPage().StartSignup(registrationUser);
        registerPage.CreateAccount(registrationUser);
        Assert.That(
            HomePage.IsAccountCreatedVisible(),
            Is.True,
            "The precondition account should be created successfully before validating the invalid login flow.");

        HomePage.ContinueAfterAccountAction();
        HomePage.Logout();

        var validUser = LoginUserFactory.CreateFromRegistration(registrationUser);
        var invalidUser = LoginUserFactory.CreateInvalidPasswordUser(validUser);
        var loginPage = HomePage.GoToLoginPage();
        loginPage.Login(invalidUser);

        Assert.Multiple(() =>
        {
            Assert.That(
                loginPage.GetLoginErrorText(),
                Is.EqualTo(Messages.InvalidLogin),
                "The login page should display the expected error message for invalid credentials.");
            Assert.That(
                loginPage.IsCurrentPage(),
                Is.True,
                "The user should remain on the Signup / Login page after a failed login attempt.");
            Assert.That(
                HomePage.IsLoggedIn(),
                Is.False,
                "The invalid login attempt should not authenticate the user.");
        });
    }
}
