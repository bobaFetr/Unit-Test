using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ECommerceTests.Infrastructure.Helpers;

public sealed class JavaScriptHelper
{
    private readonly IWebDriver _driver;

    public JavaScriptHelper(IWebDriver driver)
    {
        _driver = driver;
    }

    public void WaitForPageReady(TimeSpan timeout)
    {
        EnsureAjaxTracker();

        var wait = new DefaultWait<IWebDriver>(_driver)
        {
            Timeout = timeout,
            PollingInterval = TimeSpan.FromMilliseconds(250)
        };

        wait.IgnoreExceptionTypes(typeof(JavaScriptException), typeof(WebDriverException));
        wait.Until(_ => IsDocumentReady() && AreAjaxRequestsCompleted());
    }

    public void ScrollIntoView(IWebElement element)
    {
        ExecuteScript("arguments[0].scrollIntoView({ block: 'center', inline: 'nearest' });", element);
    }

    public void Click(IWebElement element)
    {
        ExecuteScript("arguments[0].click();", element);
    }

    public string GetInputValidationMessage(IWebElement element)
    {
        return Convert.ToString(ExecuteScript("return arguments[0].validationMessage;", element))?.Trim() ?? string.Empty;
    }

    public string GetValue(IWebElement element)
    {
        return Convert.ToString(ExecuteScript("return arguments[0].value;", element))?.Trim() ?? string.Empty;
    }

    private bool IsDocumentReady()
    {
        var state = Convert.ToString(ExecuteScript("return document.readyState;"));
        return string.Equals(state, "complete", StringComparison.OrdinalIgnoreCase);
    }

    private bool AreAjaxRequestsCompleted()
    {
        var result = ExecuteScript(
            "var jqueryDone = typeof window.jQuery === 'undefined' || window.jQuery.active === 0;" +
            "var pending = window.__pendingAjaxRequests || 0;" +
            "return jqueryDone && pending === 0;");

        return result is bool flag && flag;
    }

    private void EnsureAjaxTracker()
    {
        ExecuteScript(
            "if (!window.__aeAjaxTrackerInstalled) {" +
            "  window.__pendingAjaxRequests = 0;" +
            "  (function() {" +
            "    var originalSend = XMLHttpRequest.prototype.send;" +
            "    XMLHttpRequest.prototype.send = function() {" +
            "      window.__pendingAjaxRequests = (window.__pendingAjaxRequests || 0) + 1;" +
            "      this.addEventListener('loadend', function() {" +
            "        window.__pendingAjaxRequests = Math.max(0, (window.__pendingAjaxRequests || 1) - 1);" +
            "      }, { once: true });" +
            "      return originalSend.apply(this, arguments);" +
            "    };" +
            "    if (window.fetch) {" +
            "      var originalFetch = window.fetch;" +
            "      window.fetch = function() {" +
            "        window.__pendingAjaxRequests = (window.__pendingAjaxRequests || 0) + 1;" +
            "        return originalFetch.apply(this, arguments).finally(function() {" +
            "          window.__pendingAjaxRequests = Math.max(0, (window.__pendingAjaxRequests || 1) - 1);" +
            "        });" +
            "      };" +
            "    }" +
            "  })();" +
            "  window.__aeAjaxTrackerInstalled = true;" +
            "}");
    }

    private object? ExecuteScript(string script, params object[] arguments)
    {
        return ((IJavaScriptExecutor)_driver).ExecuteScript(script, arguments);
    }
}
