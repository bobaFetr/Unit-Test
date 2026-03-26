using System;
using System.Linq;
using ECommerceTests.Base;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ECommerceTests.Diagnostic;

[TestFixture]
[Category("Diagnostic")]
[Explicit]
[CancelAfter(20000)]
public sealed class PageInspectionTests : BaseTest
{
    /// <summary>
    /// Diagnostic test to inspect the Products page HTML structure.
    /// Run this manually to see what elements actually exist on the page.
    /// </summary>
    [Test]
    public void InspectProductsPageElements()
    {
        var productsPage = HomePage.GoToProductsPage();
        
        TestContext.WriteLine("=== PRODUCTS PAGE INSPECTION ===");
        TestContext.WriteLine($"Current URL: {Driver.Url}");
        TestContext.WriteLine($"Page Title: {Driver.Title}");
        
        // Check for search input variations
        TestContext.WriteLine("\n=== SEARCH INPUT INSPECTION ===");
        CheckElement("//input[@id='search_product']", "Original XPath");
        CheckElement("//input[@name='search']", "By name='search'");
        CheckElement("#search_product", "By ID (CSS)");
        CheckElement("input[type='search']", "By type='search'");
        CheckElement("input[placeholder*='earch']", "By placeholder containing 'earch'");
        CheckElement("//form//input[@type='text']", "Form text inputs");
        
        // List all input elements
        TestContext.WriteLine("\n=== ALL INPUT ELEMENTS ON PAGE ===");
        try
        {
            var allInputs = Driver.FindElements(By.TagName("input"));
            TestContext.WriteLine($"Total input elements found: {allInputs.Count}");
            
            foreach (var input in allInputs.Take(10))
            {
                var id = input.GetAttribute("id") ?? "N/A";
                var name = input.GetAttribute("name") ?? "N/A";
                var type = input.GetAttribute("type") ?? "N/A";
                var placeholder = input.GetAttribute("placeholder") ?? "N/A";
                var className = input.GetAttribute("class") ?? "N/A";
                
                TestContext.WriteLine($"  Input: id='{id}', name='{name}', type='{type}', placeholder='{placeholder}', class='{className}'");
            }
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Error listing inputs: {ex.Message}");
        }
        
        // Check for search button
        TestContext.WriteLine("\n=== SEARCH BUTTON INSPECTION ===");
        CheckElement("//button[@id='submit_search']", "Original button XPath");
        CheckElement("#submit_search", "Button by ID (CSS)");
        CheckElement("button[type='submit']", "Submit buttons");
        
        // List all buttons
        TestContext.WriteLine("\n=== ALL BUTTON ELEMENTS ===");
        try
        {
            var allButtons = Driver.FindElements(By.TagName("button"));
            TestContext.WriteLine($"Total button elements found: {allButtons.Count}");
            
            foreach (var button in allButtons.Take(10))
            {
                var id = button.GetAttribute("id") ?? "N/A";
                var text = button.Text ?? "N/A";
                var type = button.GetAttribute("type") ?? "N/A";
                
                TestContext.WriteLine($"  Button: id='{id}', text='{text}', type='{type}'");
            }
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Error listing buttons: {ex.Message}");
        }
        
        // Check page header
        TestContext.WriteLine("\n=== PAGE HEADER INSPECTION ===");
        CheckElement("//h2[normalize-space()='All Products']", "All Products header");
        CheckElement("h2", "Any H2 elements");
        
        // Get page source snippet
        TestContext.WriteLine("\n=== PAGE SOURCE SAMPLE (first 2000 chars) ===");
        var pageSource = Driver.PageSource;
        TestContext.WriteLine(pageSource.Substring(0, Math.Min(2000, pageSource.Length)));
        
        Assert.Pass("Diagnostic inspection complete. Review test output for details.");
    }
    
    private void CheckElement(string selector, string description)
    {
        try
        {
            IWebElement element;
            if (selector.StartsWith("//") || selector.StartsWith("("))
            {
                element = Driver.FindElement(By.XPath(selector));
            }
            else
            {
                element = Driver.FindElement(By.CssSelector(selector));
            }
            
            var isDisplayed = element.Displayed;
            var tagName = element.TagName;
            TestContext.WriteLine($"✓ FOUND ({description}): selector='{selector}', tag='{tagName}', displayed={isDisplayed}");
        }
        catch (NoSuchElementException)
        {
            TestContext.WriteLine($"✗ NOT FOUND ({description}): selector='{selector}'");
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"✗ ERROR ({description}): {ex.Message}");
        }
    }
}
