using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class LoginUserFactory
{
    public static LoginUserDto CreateFromRegistration(RegistrationUserDto user)
    {
        return new LoginUserDto
        {
            Email = user.Email,
            Password = user.Password,
            DisplayName = user.Email
        };
    }

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

    public static LoginUserDto CreateInvalidPasswordUser(LoginUserDto validUser)
    {
        return new LoginUserDto
        {
            Email = validUser.Email,
            Password = $"{validUser.Password}_invalid",
            DisplayName = validUser.DisplayName
        };
    }
}
