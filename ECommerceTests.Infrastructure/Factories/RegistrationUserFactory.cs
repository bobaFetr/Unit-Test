using System;
using ECommerceTests.Infrastructure.Configuration;
using ECommerceTests.Infrastructure.DTOs;

namespace ECommerceTests.Infrastructure.Factories;

public static class RegistrationUserFactory
{
    public static RegistrationUserDto CreateUniqueUser()
    {
        var uniqueToken = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var name = $"Student{uniqueToken}";

        return new RegistrationUserDto
        {
            Name = name,
            Email = $"student.{uniqueToken}@example.com",
            Password = "Pass123!",
            FirstName = "Student",
            LastName = "Automation",
            Company = "QA Academy",
            AddressLine1 = "123 Test Street",
            AddressLine2 = "Suite 42",
            Country = "Canada",
            State = "Ontario",
            City = "Toronto",
            ZipCode = "M5V2T6",
            MobileNumber = "+359888000111"
        };
    }

    public static RegistrationUserDto CreateExistingEmailUser()
    {
        return new RegistrationUserDto
        {
            Name = "Existing User",
            Email = TestSettings.ExistingUserEmail,
            Password = "Pass123!",
            FirstName = "Existing",
            LastName = "User",
            Company = "QA Academy",
            AddressLine1 = "123 Test Street",
            AddressLine2 = "Suite 42",
            Country = "Canada",
            State = "Ontario",
            City = "Toronto",
            ZipCode = "M5V2T6",
            MobileNumber = "+359888000111"
        };
    }

    public static RegistrationUserDto CreateInvalidEmailUser()
    {
        return new RegistrationUserDto
        {
            Name = "Invalid User",
            Email = "invalid-email-format",
            Password = "Pass123!",
            FirstName = "Invalid",
            LastName = "User",
            Company = "QA Academy",
            AddressLine1 = "123 Test Street",
            AddressLine2 = "Suite 42",
            Country = "Canada",
            State = "Ontario",
            City = "Toronto",
            ZipCode = "M5V2T6",
            MobileNumber = "+359888000111"
        };
    }
}
