# E-Commerce Test Automation - Setup Instructions

## Overview
This is a Selenium-based UI automation testing framework for https://automationexercise.com built with:
- **.NET 9** and **C# 14.0**
- **NUnit** testing framework
- **Selenium WebDriver** with Chrome
- **Page Object Model** design pattern

---

## Prerequisites
- Visual Studio 2022/2026
- .NET 9 SDK
- Chrome browser (ChromeDriver is managed automatically)

---

## Running Tests

### Option 1: Visual Studio Test Explorer
1. Open the solution in Visual Studio
2. Build the solution (Ctrl + Shift + B)
3. Open **Test Explorer** (Test → Test Explorer)
4. Run tests:
   - **All tests**: Click "Run All"
   - **Specific test**: Right-click → Run Tests
   - **Debug mode**: Right-click → Debug Tests

### Option 2: Command Line
```powershell
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~CartTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~AddProductToCart_ShouldAddExpectedProductDto"

# Run in headless mode
$env:AE_HEADLESS = "true"
dotnet test
```

---

## Environment Variables Configuration

Some tests require authentication credentials. You need to register a user account on https://automationexercise.com first.

### Required for Login/Checkout Tests:
- `AE_EMAIL` - Valid registered user email
- `AE_PASSWORD` - User password
- `AE_USERNAME` - Display name (optional)

### Optional:
- `AE_HEADLESS` - Set to `true` to run tests without opening browser UI

### Setting Environment Variables

#### PowerShell (Current Session)
```powershell
$env:AE_EMAIL = "your-email@example.com"
$env:AE_PASSWORD = "YourPassword123"
$env:AE_USERNAME = "Your Name"
```

#### PowerShell (Permanent - User Level)
```powershell
[System.Environment]::SetEnvironmentVariable("AE_EMAIL", "your-email@example.com", "User")
[System.Environment]::SetEnvironmentVariable("AE_PASSWORD", "YourPassword123", "User")
[System.Environment]::SetEnvironmentVariable("AE_USERNAME", "Your Name", "User")
```

#### Windows Command Prompt
```cmd
set AE_EMAIL=your-email@example.com
set AE_PASSWORD=YourPassword123
set AE_USERNAME=Your Name
```

#### Visual Studio (launchSettings.json)
Create/edit `.vscode/launch.json` or set in Visual Studio:
```json
{
  "profiles": {
    "Tests": {
      "environmentVariables": {
        "AE_EMAIL": "your-email@example.com",
        "AE_PASSWORD": "YourPassword123",
        "AE_USERNAME": "Your Name",
        "AE_HEADLESS": "false"
      }
    }
  }
}
```

---

## Test Categories

### Cart Tests (`CartTests.cs`)
- ✅ `AddProductToCart_ShouldAddExpectedProductDto` - Verify adding products to cart
- ✅ `RemoveProductFromCart_ShouldClearTheCart` - Verify removing products
- ✅ `Cart_ShouldKeepExpectedQuantityAndDetailsForSelectedProduct` - Verify quantity handling
- ⚠️ `CheckoutNavigation_WithAuthenticatedUser_ShouldShowExpectedOrderSummary` - **Requires credentials**

### Login Tests (`LoginTests.cs`)
- Authentication and login functionality
- **Requires environment variables**

### Registration Tests (`RegistrationTests.cs`)
- New user registration flow

### Search Tests (`SearchTests.cs`)
- Product search functionality

---

## Troubleshooting

### Issue: "Environment variables must be configured"
**Solution**: Set the `AE_EMAIL` and `AE_PASSWORD` environment variables (see above).

### Issue: Tests fail with element not found errors
**Solution**: 
- The framework now includes automatic consent banner dismissal
- It falls back to direct URL navigation if element clicking fails
- Check if the website structure has changed

### Issue: Tests running slowly
**Solution**: Set `AE_HEADLESS=true` to run without browser UI.

### Issue: ChromeDriver version mismatch
**Solution**: Update Chrome browser or the Selenium.WebDriver NuGet package.

---

## Project Structure

```
ECommerceTests/
├── Cart/                    # Shopping cart tests
├── Login/                   # Login functionality tests
├── Registration/            # User registration tests
├── Search/                  # Search functionality tests
└── Base/                    # Base test class with setup/teardown

ECommerceTests.Infrastructure/
├── Configuration/           # Test settings & environment config
├── Constants/              # URLs, messages, test data
├── Drivers/                # WebDriver factory
├── Helpers/                # Wait, JavaScript, Navigation helpers
├── Pages/                  # Page Object Model classes
├── Factories/              # Test data factories
└── Models/                 # Data transfer objects (DTOs)
```

---

## Quick Start Example

1. **Clone/open the project**
2. **Build the solution**
   ```powershell
   dotnet build
   ```

3. **Run non-authenticated tests**
   ```powershell
   dotnet test --filter "FullyQualifiedName~AddProductToCart"
   ```

4. **For authenticated tests, set credentials**
   ```powershell
   $env:AE_EMAIL = "test@example.com"
   $env:AE_PASSWORD = "Test123!"
   dotnet test --filter "FullyQualifiedName~Checkout"
   ```

---

## Key Features

✅ **Automatic consent banner handling** - Dismisses Google Consent and cookie popups  
✅ **Fallback navigation** - Direct URL navigation if element clicking fails  
✅ **Robust wait strategies** - Waits for page ready, AJAX, and dynamic content  
✅ **Headless mode support** - Run tests without browser UI  
✅ **Page Object Model** - Clean, maintainable test code  
✅ **Comprehensive assertions** - Detailed failure messages  

---

## Support

For issues or questions:
1. Check the error message in Test Explorer
2. Review the stack trace for specific line numbers
3. Run in debug mode to step through test execution
4. Ensure environment variables are set correctly

---

## Recent Fixes Applied

- ✅ Added automatic Google Consent banner dismissal
- ✅ Improved element location with multiple fallback selectors
- ✅ Added direct URL navigation as final fallback
- ✅ Enhanced wait strategies for dynamic content
- ✅ Better error handling and recovery
