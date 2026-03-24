# ECommerceTests Documentation

## 1. Project Overview

This solution is a complete UI test automation framework for the demo e-commerce website:

`https://automationexercise.com/`

The framework is built with:

- C#
- NUnit
- Selenium WebDriver
- Chrome browser
- Page Object Model
- DTOs
- Factory classes
- JavaScript execution
- Explicit waits

The goal of the framework is to provide stable, readable, and maintainable end-to-end system tests for the following website areas:

- Login
- Registration
- Cart
- Search

The implementation follows the academic requirements by using:

- separate infrastructure and test projects
- DTO-to-DTO comparisons
- factories for test data creation
- page inheritance and test inheritance
- flexible XPath locators
- XML documentation on all tests
- environment variables for credentials
- explicit waits and JavaScript-based page readiness checks

---

## 2. Solution Structure

### Solution

- `ECommerceTests.sln`

### Project 1: `ECommerceTests.Infrastructure`

This project contains all reusable automation framework components:

- Drivers
- Pages
- DTOs
- Factories
- Helpers
- Constants
- Configuration

### Project 2: `ECommerceTests`

This project contains the actual test suites:

- Base test class
- Login tests
- Registration tests
- Cart tests
- Search tests

---

## 3. High-Level Architecture

The framework is layered so responsibilities are separated clearly.

### Infrastructure layer responsibilities

- create and configure WebDriver
- manage application URLs and framework settings
- store reusable wait and JavaScript helpers
- model pages using Page Object Model
- model test data with DTOs
- generate test data through factory classes

### Test layer responsibilities

- define test cases and expected business behavior
- compose reusable page object actions
- compare actual and expected DTOs
- perform meaningful assertions
- manage setup and teardown through inheritance

---

## 4. Design Patterns Used

### 4.1 Page Object Model

Every major page in the website is represented by a class.  
Each page object:

- stores page locators
- exposes page-specific actions
- hides Selenium details from tests
- keeps tests readable and maintainable

Implemented page objects:

- `BasePage`
- `HomePage`
- `LoginPage`
- `SignupPage`
- `ProductsPage`
- `ProductDetailsPage`
- `CartPage`
- `CheckoutPage`

### 4.2 Factory Pattern

Factories are used to generate DTOs for expected test data and input data.  
This keeps tests clean and avoids hardcoding data directly inside test methods.

Implemented factories:

- `LoginUserFactory`
- `RegistrationUserFactory`
- `ProductFactory`
- `SearchOptionsFactory`

### 4.3 Inheritance

Inheritance is used in both pages and tests.

#### Page inheritance

All pages inherit from `BasePage`, which centralizes:

- driver access
- wait handling
- JavaScript helper access
- common actions like click, type, select, and page ready checks

#### Test inheritance

All tests inherit from `BaseTest`, which centralizes:

- WebDriver setup
- browser teardown
- login helper logic
- cart cleanup logic

---

## 5. Configuration and Constants

### `Urls.cs`

Stores reusable application routes and the base URL:

- base URL
- login URL
- products URL
- cart URL
- checkout URL
- signup URL

This ensures the framework does not hardcode the same URLs in multiple places.

### `Messages.cs`

Stores expected visible messages used in assertions, for example:

- invalid login message
- existing email message
- search results heading

This improves readability and makes message maintenance easier.

### `TestSettings.cs`

Stores framework runtime settings such as:

- explicit wait timeout
- page ready timeout
- page load timeout
- command timeout
- headless browser flag
- credential access through environment variables

It also provides validation for required environment variables like:

- `AE_EMAIL`
- `AE_PASSWORD`

---

## 6. Driver Management

### `DriverFactory.cs`

`DriverFactory` is responsible for creating a configured `ChromeDriver`.

It applies:

- browser arguments for stability
- consistent browser window size
- optional headless execution through `AE_HEADLESS`
- zero implicit wait
- controlled page load timeout

Using a driver factory keeps browser setup in one place and avoids duplication.

---

## 7. DTO Layer

DTOs are used so tests compare meaningful objects instead of isolated primitive values whenever possible.

### `LoginUserDto`

Represents login credentials:

- email
- password
- display name

### `RegistrationUserDto`

Represents account creation data:

- account identity information
- password
- date of birth
- address information
- mobile number

### `ProductDto`

Represents product data:

- name
- price
- quantity
- category
- brand

### `SearchOptionsDto`

Represents search input and expected results:

- search term
- expected product DTO collection
- expectation flag for results

### Why records were used

The DTOs are implemented as C# `record` types.  
This gives clean value-based equality, which is ideal for:

- comparing expected and actual product objects
- comparing collections of DTOs
- avoiding verbose primitive-level assertions

---

## 8. Factory Layer

### `LoginUserFactory`

Creates:

- valid login user from environment variables
- invalid login user with wrong password

### `RegistrationUserFactory`

Creates:

- unique registration user
- existing email registration user
- invalid email registration user

For stability, unique emails are generated automatically with a UTC timestamp so registration tests do not fail because of reused emails.

### `ProductFactory`

Creates expected DTOs for products used in cart and search assertions, such as:

- Blue Top
- Men Tshirt
- Winter Top
- Summer White Top

### `SearchOptionsFactory`

Creates reusable search scenarios, including:

- exact product name
- partial name
- different casing
- non-existing product

This keeps test data separate from test behavior.

---

## 9. Helper Layer

### `WaitHelper.cs`

This class wraps explicit wait logic using:

- `WebDriverWait`
- `SeleniumExtras.WaitHelpers`

Supported actions include:

- wait until element visible
- wait until element clickable
- wait until all elements visible
- wait until element invisible
- wait using custom conditions

This satisfies the requirement to use explicit waits instead of `Thread.Sleep`.

### `JavaScriptHelper.cs`

This helper adds JavaScript-based actions and page readiness logic.

It supports:

- waiting for `document.readyState == "complete"`
- waiting for AJAX completion
- JavaScript clicking when standard clicking is intercepted
- scroll into view
- retrieving browser validation messages
- retrieving input values

#### AJAX readiness strategy

The helper injects a lightweight tracker for:

- `XMLHttpRequest`
- `fetch`

This allows the framework to wait until pending asynchronous requests are finished.

### `NavigationHelper.cs`

This helper centralizes navigation and ensures page readiness after:

- normal navigation
- refresh
- browser back navigation

---

## 10. Page Object Documentation

### `BasePage`

This is the foundation for all page classes.

Provided shared behavior:

- open page by relative URL
- click elements safely
- click with JavaScript
- type text
- select dropdown values
- get text
- get values
- get browser validation messages
- wait for page ready
- convert strings safely into XPath literals

This keeps child page classes much smaller and more readable.

### `HomePage`

Main responsibilities:

- navigate to login page
- navigate to products page
- navigate to cart page
- verify logged-in state
- read logged-in username
- delete created accounts
- continue after account created or deleted messages

### `LoginPage`

Main responsibilities:

- login with existing credentials
- start signup
- validate login page state
- retrieve invalid login error
- retrieve duplicate email error
- retrieve browser validation message for invalid signup email

### `SignupPage`

Main responsibilities:

- verify account information page is displayed
- fill full account creation form
- submit account creation
- verify the locked email passed from the previous page

### `ProductsPage`

Main responsibilities:

- verify products page loaded
- perform search
- submit empty search
- retrieve displayed product DTOs
- add product to cart from list page
- continue shopping from cart modal
- open cart from modal
- open product details page

### `ProductDetailsPage`

Main responsibilities:

- read product details as DTO
- set quantity
- add product to cart
- open cart from modal

### `CartPage`

Main responsibilities:

- retrieve cart rows as DTOs
- remove a product
- verify empty cart
- clear cart for cleanup
- proceed to checkout

### `CheckoutPage`

Main responsibilities:

- verify checkout page state
- retrieve order summary as DTOs
- enter order comment
- verify place order button visibility

---

## 11. Base Test Workflow

### `BaseTest.cs`

`BaseTest` controls common test lifecycle behavior.

#### `OneTimeSetUp`

Checks that the framework has a configured base URL.

#### `SetUp`

- creates ChromeDriver
- opens the home page

#### `TearDown`

- attempts cart cleanup
- quits and disposes the driver

#### Shared helper methods

- `EnsureExistingLoginCredentialsAreConfigured()`
- `LoginWithExistingUser()`
- `DeleteCurrentlyLoggedInAccountIfPossible()`

This keeps all test classes consistent and reduces duplicated Selenium logic.

---

## 12. Test Coverage Documentation

## 12.1 Login Tests

File:

- `ECommerceTests/Login/LoginTests.cs`

Implemented scenarios:

- valid login with existing credentials
- invalid login with wrong password

What is verified:

- page opens correctly
- user is authenticated successfully
- logout option is visible
- logged-in label is visible
- invalid credentials show the correct error
- failed login does not authenticate the user

Requirement coverage:

- DTO usage
- factory usage
- meaningful assertions
- multiple assertions
- XML summaries
- flexible XPath locators

---

## 12.2 Registration Tests

File:

- `ECommerceTests/Registration/RegistrationTests.cs`

Implemented scenarios:

- successful registration with unique generated email
- registration with already existing email
- registration with invalid email format

What is verified:

- account information page opens
- locked email matches the registration DTO
- account created message is displayed
- user is logged in after registration
- logged-in username matches expected DTO data
- account deletion works
- existing email message appears
- invalid email is rejected by the browser

Stability note:

The happy-path registration scenario generates unique emails automatically, so repeated executions do not collide with previous runs.

---

## 12.3 Cart Tests

File:

- `ECommerceTests/Cart/CartTests.cs`

Implemented scenarios:

- add product to cart
- remove product from cart
- verify product quantity and details in cart
- verify checkout navigation for authenticated user

What is verified:

- product DTO in cart equals expected DTO
- removing product empties cart
- selected quantity is preserved
- product details DTO matches expected product details
- checkout page loads successfully
- order summary contains expected DTO

Requirement coverage:

- DTO-to-DTO comparison
- factory-generated expected data
- multiple assertions
- independent test flow

---

## 12.4 Search Tests

File:

- `ECommerceTests/Search/SearchTests.cs`

Implemented scenarios:

- search with exact product name
- search with partial name
- search with different casing
- search with empty input
- search with non-existing product
- verify search results remain correct after interaction

Data-driven implementation:

The positive search scenarios are implemented with `TestCaseSource`, satisfying the requirement for at least one data-driven test.

What is verified:

- search results heading visibility
- result DTO collections
- empty input behavior remains usable
- non-existing search returns empty result collection
- results stay stable after add-to-cart interaction
- search term remains present after interaction

---

## 13. Locator Strategy

The framework uses flexible XPath locators based on meaningful DOM context instead of fragile absolute paths.

Locator strategy includes:

- ids when available
- `data-qa` attributes when available
- form context
- visible text
- navigation link text
- section-based product matching

Examples of resilient locator patterns used:

- login email field by `data-qa` or `name`
- signup email field scoped to signup form
- product add-to-cart button located by product card context
- cart remove button located by matching product row text
- checkout product row located by row id pattern

This improves resilience when styling or layout changes occur.

---

## 14. Wait and Stability Strategy

The solution was written to support running the tests together reliably.

Main stability measures:

- no implicit waits
- explicit waits for visibility and clickability
- page readiness wait after navigation and action transitions
- JavaScript wait for document completion
- AJAX completion tracking
- JavaScript click fallback when normal click is intercepted
- cart cleanup in teardown
- unique email generation for registration
- test data factories for consistency

These decisions reduce flaky behavior that usually appears in Selenium projects.

---

## 15. Assertion Strategy

The framework follows several assertion rules:

- every assertion has a failure message
- `Assert.Multiple` is used where more than one thing is verified
- DTO comparisons are preferred over primitive-only checks
- assertions validate behavior, not just element presence

Examples:

- comparing expected and actual `ProductDto`
- checking login state and logout visibility together
- checking search heading and result DTO collection together

---

## 16. Credential Handling

The valid login user is not hardcoded.

Required environment variables:

- `AE_EMAIL`
- `AE_PASSWORD`

Optional:

- `AE_USERNAME`
- `AE_HEADLESS`

This satisfies the requirement to store credentials outside the source code.

---

## 17. Build and Run Instructions

### Required NuGet packages

The solution references:

- `Selenium.WebDriver`
- `Selenium.Support`
- `DotNetSeleniumExtras.WaitHelpers`
- `NUnit`
- `NUnit3TestAdapter`
- `Microsoft.NET.Test.Sdk`

### Set environment variables in PowerShell

```powershell
[Environment]::SetEnvironmentVariable("AE_EMAIL", "your-valid-email@example.com", "User")
[Environment]::SetEnvironmentVariable("AE_PASSWORD", "your-valid-password", "User")
[Environment]::SetEnvironmentVariable("AE_USERNAME", "Optional Display Name", "User")
[Environment]::SetEnvironmentVariable("AE_HEADLESS", "true", "User")
```

Restart the terminal or IDE after setting them if needed.

### Restore packages

```powershell
dotnet restore ECommerceTests.sln
```

### Build the solution

```powershell
dotnet build ECommerceTests.sln
```

### Run all tests

```powershell
dotnet test ECommerceTests.sln
```

### Run a single test class

```powershell
dotnet test ECommerceTests.sln --filter "FullyQualifiedName~ECommerceTests.Login.LoginTests"
```

---

## 18. Notes and Assumptions

- The framework targets the real live site `https://automationexercise.com/`.
- Search behavior on the live site is assumed to be case-insensitive based on the implemented test coverage requirement.
- The authenticated checkout test requires valid existing credentials in environment variables.
- Chrome and a compatible ChromeDriver setup must be available on the machine.
- The happy-path registration test deletes the newly created account after validation to keep the environment clean.

---

## 19. How to Extend the Framework

To add a new test area:

1. create or update the relevant DTO
2. add factory methods for test data
3. add or extend page objects
4. keep Selenium calls inside page objects
5. add test scenarios in the test project
6. use DTO comparisons and meaningful assertions

Recommended extension examples:

- logout tests
- contact us tests
- subscription tests
- payment form negative scenarios
- category and brand filter tests

---

## 20. Submission Summary

This project satisfies the requested academic requirements by providing:

- a 2-project Selenium NUnit solution
- Page Object Model implementation
- DTOs and factories
- JavaScript-based readiness waiting
- explicit waits with SeleniumExtras wait helpers
- XML summaries on tests
- meaningful assertions with messages
- multiple assertions where needed
- data-driven search testing
- environment-variable credential handling
- reusable infrastructure with inheritance

If needed, this documentation can also be adapted into:

- a shorter student submission report
- a formal `README` for GitHub
- a Word/PDF report structure
- an architecture explanation for presentation slides
