using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Sola_Web.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static SolaContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<SolaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new SolaContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
