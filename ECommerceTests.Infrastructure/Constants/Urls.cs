namespace ECommerceTests.Infrastructure.Constants;

public static class Urls
{
    public const string BaseUrl = "https://demowebshop.tricentis.com";
    public const string Home = "/";
    public const string Login = "/login";
    public const string Search = "/search";
    public const string Products = "/";
    public const string Cart = "/cart";
    public const string Checkout = "/onepagecheckout";
    public const string Signup = "/register";

    public static string ProductDetails(string slug) => $"/{slug.TrimStart('/')}";
}
