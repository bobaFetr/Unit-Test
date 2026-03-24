using System;
using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.Constants;
using OpenQA.Selenium;

namespace ECommerceTests.Infrastructure.Helpers;

public sealed class NavigationHelper
{
    private readonly IWebDriver _driver;
    private readonly JavaScriptHelper _javaScriptHelper;

    public NavigationHelper(IWebDriver driver)
    {
        _driver = driver;
        _javaScriptHelper = new JavaScriptHelper(driver);
    }

    public void NavigateTo(string relativeOrAbsoluteUrl)
    {
        var destination = BuildUrl(relativeOrAbsoluteUrl);
        _driver.Navigate().GoToUrl(destination);
        _javaScriptHelper.WaitForPageReady(TestSettings.PageReadyTimeout);
    }

    public void Refresh()
    {
        _driver.Navigate().Refresh();
        _javaScriptHelper.WaitForPageReady(TestSettings.PageReadyTimeout);
    }

    public void GoBack()
    {
        _driver.Navigate().Back();
        _javaScriptHelper.WaitForPageReady(TestSettings.PageReadyTimeout);
    }

    private static string BuildUrl(string relativeOrAbsoluteUrl)
    {
        if (Uri.TryCreate(relativeOrAbsoluteUrl, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        return $"{Urls.BaseUrl.TrimEnd('/')}/{relativeOrAbsoluteUrl.TrimStart('/')}";
    }
}
