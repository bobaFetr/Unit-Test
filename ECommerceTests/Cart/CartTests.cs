using System.Linq;
using ECommerceTests.Base;
using ECommerceTests.Infrastructure.Factories;
using NUnit.Framework;

namespace ECommerceTests.Cart;

[TestFixture]
public sealed class CartTests : BaseTest
{
    /// <summary>
    /// Steps:
    /// 1. Open the Products page.
    /// 2. Add the Blue Top product to the cart from the product listing.
    /// 3. Open the cart and compare the cart row DTO with the expected DTO.
    /// Expected Result:
    /// The cart contains the expected product with the correct name, price, and default quantity.
    /// </summary>
    [Test]
    public void AddProductToCart_ShouldAddExpectedProductDto()
    {
        var expectedProduct = ProductFactory.CreateBlueTop();
        var productsPage = HomePage.GoToProductsPage();

        productsPage.AddProductToCart(expectedProduct.Name);
        var cartPage = productsPage.ViewCartFromModal();
        var actualProducts = cartPage.GetProducts();
        var actualProduct = actualProducts.Single(product => product.Name == expectedProduct.Name);

        Assert.Multiple(() =>
        {
            Assert.That(
                actualProducts,
                Has.Count.EqualTo(1),
                "The cart should contain exactly one product after adding a single item to an empty cart.");
            Assert.That(
                actualProduct,
                Is.EqualTo(expectedProduct),
                "The cart product DTO should match the expected DTO for the added product.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Products page.
    /// 2. Add a product to the cart and open the cart.
    /// 3. Remove the product from the cart.
    /// 4. Verify the cart becomes empty.
    /// Expected Result:
    /// The product is removed successfully and the cart no longer contains any items.
    /// </summary>
    [Test]
    public void RemoveProductFromCart_ShouldClearTheCart()
    {
        var expectedProduct = ProductFactory.CreateBlueTop();
        var productsPage = HomePage.GoToProductsPage();

        productsPage.AddProductToCart(expectedProduct.Name);
        var cartPage = productsPage.ViewCartFromModal();
        cartPage.RemoveProduct(expectedProduct.Name);

        Assert.Multiple(() =>
        {
            Assert.That(
                cartPage.IsEmpty(),
                Is.True,
                "The cart should be empty after the only product is removed.");
            Assert.That(
                cartPage.GetProducts(),
                Is.Empty,
                "The cart product collection should be empty after removing the product.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Open the Blue Top product details page from the Products page.
    /// 2. Change the quantity to 2 and add the product to the cart.
    /// 3. Compare the product details DTO and the cart DTO with the expected DTOs from the factory.
    /// Expected Result:
    /// The product details and the cart both reflect the expected name, price, category, brand, and quantity.
    /// </summary>
    [TestCase(2)]
    public void Cart_ShouldKeepExpectedQuantityAndDetailsForSelectedProduct(int quantity)
    {
        var expectedCartProduct = ProductFactory.CreateBlueTop(quantity.ToString());
        var expectedDetailsProduct = ProductFactory.CreateBlueTopDetails(quantity.ToString());
        var productsPage = HomePage.GoToProductsPage();
        var productDetailsPage = productsPage.ViewProduct(expectedCartProduct.Name);

        productDetailsPage.SetQuantity(quantity);
        var actualDetailsProduct = productDetailsPage.GetProductDetails(quantity.ToString());
        productDetailsPage.AddToCart();
        var cartPage = productDetailsPage.ViewCartFromModal();
        var actualCartProduct = cartPage.GetProducts().Single();

        Assert.Multiple(() =>
        {
            Assert.That(
                actualDetailsProduct,
                Is.EqualTo(expectedDetailsProduct),
                "The product details DTO should match the expected DTO before the product is added to the cart.");
            Assert.That(
                actualCartProduct,
                Is.EqualTo(expectedCartProduct),
                "The cart product DTO should match the expected DTO after adding the selected quantity.");
        });
    }

    /// <summary>
    /// Steps:
    /// 1. Log in with the existing valid user.
    /// 2. Add a product to the cart and proceed to checkout.
    /// 3. Verify the checkout page is displayed and compare the order summary DTO with the expected product DTO.
    /// Expected Result:
    /// The user reaches checkout successfully and the order summary contains the expected product details.
    /// </summary>
    [Test]
    public void CheckoutNavigation_WithAuthenticatedUser_ShouldShowExpectedOrderSummary()
    {
        LoginWithExistingUser();
        var expectedProduct = ProductFactory.CreateMenTshirt();
        var productsPage = HomePage.GoToProductsPage();

        productsPage.AddProductToCart(expectedProduct.Name);
        var cartPage = productsPage.ViewCartFromModal();
        var checkoutPage = cartPage.ProceedToCheckout();
        var checkoutProducts = checkoutPage.GetOrderProducts();

        Assert.Multiple(() =>
        {
            Assert.That(
                checkoutPage.IsCurrentPage(),
                Is.True,
                "Proceeding to checkout while authenticated should open the checkout page.");
            Assert.That(
                checkoutPage.IsPlaceOrderVisible(),
                Is.True,
                "The Place Order action should be available on the checkout page.");
            Assert.That(
                checkoutProducts,
                Does.Contain(expectedProduct),
                "The checkout order summary should contain the expected product DTO.");
        });
    }
}
