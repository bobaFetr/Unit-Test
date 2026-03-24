namespace ECommerceTests.Infrastructure.Constants;

public static class Urls
{
    public const string BaseUrl = "https://automationexercise.com";
    public const string Login = "/login";
    public const string Products = "/products";
    public const string Cart = "/view_cart";
    public const string Checkout = "/checkout";
    public const string Signup = "/signup";

    public static string ProductDetails(int productId) => $"/product_details/{productId}";
}
