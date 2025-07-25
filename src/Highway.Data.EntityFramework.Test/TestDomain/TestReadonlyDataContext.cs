using Common.Logging;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class TestReadonlyDataContext(DbContextOptions options, ILog logger) : ReadonlyDataContext(options, null, logger);