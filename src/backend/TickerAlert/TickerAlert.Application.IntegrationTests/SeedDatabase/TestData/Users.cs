using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Authentication;

namespace TickerAlert.Application.IntegrationTests.SeedDatabase.TestData;

public static class Users
{
    public static User CreateTestUser()
    {
        return User.CreateUser(
            Guid.Parse("e2b0b4e1-21c7-4d2b-b45c-9c2b9a9f4e2a"), 
            "testuser@gmail.com", 
            PasswordHelper.HashPassword("testuser")
        );
    }
}