using Common.Logging;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class TestReadonlyDataContext : ReadonlyDataContext
{
    public TestReadonlyDataContext(DbContextOptions options, ILog logger)
        : base(options, null, null, logger)
    {
    }
}