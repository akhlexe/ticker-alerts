using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure.UnitTests.Common.Persistence;

public static class DbContextInMemory
{
    public static ApplicationDbContext Create()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(dbContextOptions);

        context.Database.EnsureCreated();

        return context;
    }
}