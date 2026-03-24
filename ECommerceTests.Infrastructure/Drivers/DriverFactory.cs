using System;
using ECommerceTests.Infrastructure.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ECommerceTests.Infrastructure.Drivers;

public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--window-size=1920,1080");

        if (TestSettings.RunHeadless)
        {
            options.AddArgument("--headless=new");
        }

        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        service.SuppressInitialDiagnosticInformation = true;

        var driver = new ChromeDriver(service, options, TestSettings.DriverCommandTimeout);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        driver.Manage().Timeouts().PageLoad = TestSettings.PageLoadTimeout;

        return driver;
    }
}
