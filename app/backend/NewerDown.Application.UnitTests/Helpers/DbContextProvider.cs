using Microsoft.EntityFrameworkCore;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Helpers;

public class DbContextProvider
{
    public ApplicationDbContext BuildDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}