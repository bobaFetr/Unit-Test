using System;
using System.Collections.Generic;
using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ECommerceTests.Infrastructure.Pages;

public abstract class BasePage
{
    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        WaitHelper = new WaitHelper(driver, TestSettings.ExplicitWaitTimeout);
        JavaScriptHelper = new JavaScriptHelper(driver);
        NavigationHelper = new NavigationHelper(driver);
    }

    protected IWebDriver Driver { get; }

    protected WaitHelper WaitHelper { get; }

    protected JavaScriptHelper JavaScriptHelper { get; }

    protected NavigationHelper NavigationHelper { get; }

    public abstract string RelativeUrl { get; }

    public virtual void Open()
    {
        NavigationHelper.NavigateTo(RelativeUrl);
        WaitForPageReady();
    }

    protected void WaitForPageReady()
    {
        WaitHelper.WaitForPageReady(JavaScriptHelper);
    }

    protected IWebElement FindVisible(By locator)
    {
        WaitForPageReady();
        return WaitHelper.UntilVisible(locator);
    }

    protected IReadOnlyCollection<IWebElement> FindAllVisible(By locator)
    {
        WaitForPageReady();
        return WaitHelper.UntilAllVisible(locator);
    }

    protected void Click(By locator)
    {
        var element = WaitHelper.UntilClickable(locator);
        JavaScriptHelper.ScrollIntoView(element);

        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            JavaScriptHelper.Click(element);
        }

        WaitForPageReady();
    }

    protected void ClickWithJavaScript(By locator)
    {
        var element = WaitHelper.UntilVisible(locator);
        JavaScriptHelper.ScrollIntoView(element);
        JavaScriptHelper.Click(element);
        WaitForPageReady();
    }

    protected void Type(By locator, string value)
    {
        var element = FindVisible(locator);
        JavaScriptHelper.ScrollIntoView(element);
        element.Clear();
        element.SendKeys(value);
    }

    protected void SelectByText(By locator, string visibleText)
    {
        var element = FindVisible(locator);
        var select = new SelectElement(element);

        try
        {
            select.SelectByText(visibleText);
        }
        catch (NoSuchElementException)
        {
            try
            {
                select.SelectByValue(visibleText);
            }
            catch
            {
                var options = select.Options;
                foreach (var option in options)
                {
                    if (option.Text.Trim() == visibleText || 
                        option.GetAttribute("value") == visibleText ||
                        option.Text.Contains(visibleText))
                    {
                        option.Click();
                        return;
                    }
                }
                throw;
            }
        }
    }

    protected string GetText(By locator)
    {
        return FindVisible(locator).Text.Trim();
    }

    protected string GetValue(By locator)
    {
        return JavaScriptHelper.GetValue(FindVisible(locator));
    }

    protected string GetValidationMessage(By locator)
    {
        return JavaScriptHelper.GetInputValidationMessage(FindVisible(locator));
    }

    protected bool IsElementDisplayed(By locator, int timeoutInSeconds = 5)
    {
        return WaitHelper.IsElementDisplayed(locator, TimeSpan.FromSeconds(timeoutInSeconds));
    }

    protected static string ToXPathLiteral(string value)
    {
        if (!value.Contains('\''))
        {
            return $"'{value}'";
        }

        if (!value.Contains('"'))
        {
            return $"\"{value}\"";
        }

        var parts = value.Split('\'');
        return $"concat('{string.Join("',\"'\",'", parts)}')";
    }

    protected void DismissConsentIfPresent()
    {
        try
        {
            var consentSelectors = new[]
            {
                By.XPath("//button[contains(., 'Consent') or contains(., 'Accept') or contains(., 'Agree')]"),
                By.XPath("//button[@aria-label='Consent' or @aria-label='Accept' or @aria-label='Close']"),
                By.CssSelector("button[class*='consent'], button[class*='accept'], button[class*='agree']"),
                By.XPath("//div[@id='dismiss-button']//button"),
                By.XPath("//iframe[contains(@src, 'consent') or contains(@name, 'consent')]"),
                By.XPath("//div[contains(@class, 'fc-consent')]//button"),
                By.CssSelector(".fc-cta-consent, .fc-primary-button")
            };

            foreach (var selector in consentSelectors)
            {
                try
                {
                    if (selector.ToString().Contains("iframe"))
                    {
                        var iframes = Driver.FindElements(selector);
                        foreach (var iframe in iframes)
                        {
                            try
                            {
                                Driver.SwitchTo().Frame(iframe);
                                var consentButton = Driver.FindElements(
                                    By.XPath("//button[contains(., 'Consent') or contains(., 'Accept')]"));
                                if (consentButton.Count > 0)
                                {
                                    consentButton[0].Click();
                                    Driver.SwitchTo().DefaultContent();
                                    System.Threading.Thread.Sleep(500);
                                    return;
                                }
                                Driver.SwitchTo().DefaultContent();
                            }
                            catch
                            {
                                Driver.SwitchTo().DefaultContent();
                            }
                        }
                    }
                    else if (IsElementDisplayed(selector, 2))
                    {
                        var element = WaitHelper.UntilVisible(selector);
                        element.Click();
                        System.Threading.Thread.Sleep(500);
                        return;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        catch
        {
        }
    }
}
