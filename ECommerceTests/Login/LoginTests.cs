using ECommerceTests.Base;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Login;

[TestFixture]
public sealed class LoginTests : BaseTest
{
    /// <summary>
    /// Steps:
    /// 1. Open the Signup / Login page.
    /// 2. Log in with the valid credentials stored in the environment variables.
    /// 3. Verify the home page reflects a successful authenticated state.
    /// Expected Result:
    /// The user is logged in successfully, the logout option is visible, and the logged-in user label is populated.
    /// </summary>
    [Test]
    public void ValidLogin_WithExistingCredentials_ShouldLogInSuccessfully()
    {
        EnsureExistingLoginCredentialsAreConfigured();
        var validUser = LoginUserFactory.CreateFromEnvironment();
        var loginPage = HomePage.GoToLoginPage();

        Assert.That(
            loginPage.IsCurrentPage(),
            Is.True,
            "The Signup / Login page should display both login and signup sections before login is attempted.");

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
                Is.Not.Empty,
                "The logged-in user banner should contain the authenticated user's display name.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Signup / Login page.
    /// 2. Attempt to log in with a valid email and an intentionally wrong password.
    /// 3. Verify the login attempt is rejected with a meaningful error.
    /// Expected Result:
    /// The user remains unauthenticated and the incorrect credentials error message is displayed.
    /// </summary>
    [Test]
    public void InvalidLogin_WithWrongPassword_ShouldShowValidationMessage()
    {
        EnsureExistingLoginCredentialsAreConfigured();
        var invalidUser = LoginUserFactory.CreateInvalidPasswordUser();
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
