using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ECommerceTests.Infrastructure.Helpers;

public sealed class WaitHelper
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public WaitHelper(IWebDriver driver, TimeSpan timeout)
    {
        _driver = driver;
        _wait = new WebDriverWait(new SystemClock(), driver, timeout, TimeSpan.FromMilliseconds(250));
        _wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementClickInterceptedException));
    }

    public IWebElement UntilVisible(By locator)
    {
        return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    public IWebElement UntilClickable(By locator)
    {
        return _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public IReadOnlyCollection<IWebElement> UntilAllVisible(By locator)
    {
        return _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
    }

    public bool UntilInvisible(By locator)
    {
        return _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    public bool Until(Func<IWebDriver, bool> condition)
    {
        return _wait.Until(condition);
    }

    public void WaitForPageReady(JavaScriptHelper javaScriptHelper)
    {
        javaScriptHelper.WaitForPageReady(_wait.Timeout);
    }

    public bool IsElementDisplayed(By locator, TimeSpan timeout)
    {
        try
        {
            var shortWait = new WebDriverWait(new SystemClock(), _driver, timeout, TimeSpan.FromMilliseconds(200));
            shortWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            shortWait.Until(ExpectedConditions.ElementExists(locator));
            return _driver.FindElement(locator).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
