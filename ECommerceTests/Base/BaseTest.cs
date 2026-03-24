using System;
using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.Constants;
using ECommerceTests.Infrastructure.Drivers;
using ECommerceTests.Infrastructure.Factories;
using ECommerceTests.Infrastructure.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ECommerceTests.Base;

[TestFixture]
public abstract class BaseTest
{
    protected IWebDriver Driver = null!;
    protected HomePage HomePage = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Assert.That(Urls.BaseUrl, Is.Not.Empty, "The framework base URL must be configured before the tests start.");
    }

    [SetUp]
    public virtual void SetUp()
    {
        Driver = DriverFactory.CreateChromeDriver();
        HomePage = new HomePage(Driver);
        HomePage.Open();
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            CleanupCartIfPossible();
        }
        catch
        {
        }
        finally
        {
            try
            {
                Driver?.Quit();
            }
            catch
            {
            }

            Driver?.Dispose();
        }
    }

    protected void EnsureExistingLoginCredentialsAreConfigured()
    {
        Assert.That(
            TestSettings.HasExistingLoginCredentials,
            Is.True,
            "Environment variables AE_EMAIL and AE_PASSWORD must be configured before running this test.");
    }

    protected void LoginWithExistingUser()
    {
        EnsureExistingLoginCredentialsAreConfigured();

        var loginPage = new LoginPage(Driver);
        var user = LoginUserFactory.CreateFromEnvironment();
        loginPage.Open();
        loginPage.Login(user);

        Assert.That(
            HomePage.IsLoggedIn(),
            Is.True,
            "The valid existing user should be authenticated successfully before the test continues.");
    }

    protected void DeleteCurrentlyLoggedInAccountIfPossible()
    {
        if (Driver is null)
        {
            return;
        }

        var page = new HomePage(Driver);

        if (!page.CanDeleteCurrentAccount())
        {
            return;
        }

        page.DeleteCurrentAccount();

        Assert.That(
            page.IsAccountDeletedVisible(),
            Is.True,
            "Account cleanup should show a successful deletion confirmation message.");

        page.ContinueAfterAccountAction();
    }

    private void CleanupCartIfPossible()
    {
        if (Driver is null)
        {
            return;
        }

        var cartPage = new CartPage(Driver);
        cartPage.Open();

        if (!cartPage.IsEmpty())
        {
            cartPage.ClearCart();
        }
    }
}
