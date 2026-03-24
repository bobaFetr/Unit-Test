using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class LoginUserFactory
{
    public static LoginUserDto CreateFromEnvironment()
    {
        return new LoginUserDto
        {
            Email = TestSettings.ExistingUserEmail,
            Password = TestSettings.ExistingUserPassword,
            DisplayName = TestSettings.ExistingUserDisplayName
        };
    }

    public static LoginUserDto CreateInvalidPasswordUser()
    {
        return new LoginUserDto
        {
            Email = TestSettings.ExistingUserEmail,
            Password = $"{TestSettings.ExistingUserPassword}_invalid"
        };
    }
}
