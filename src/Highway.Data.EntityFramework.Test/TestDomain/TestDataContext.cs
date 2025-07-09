using Common.Logging;
using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class TestDataContext : DataContext
{
    public TestDataContext(DbContextOptions options, IMappingConfiguration mapping, ILog logger)
        : base(options, logger, mapping, null)
    {
    }
}