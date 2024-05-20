using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Application.UnitTests.Common.Persistence;

public static class DbContextInMemory
{
    public static IApplicationDbContext Create()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(dbContextOptions);

        context.Database.EnsureCreated();

        return context;
    }
}