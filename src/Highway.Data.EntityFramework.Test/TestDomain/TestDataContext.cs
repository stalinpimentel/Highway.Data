using Common.Logging;
using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class TestDataContext(DbContextOptions options, IMappingConfiguration mapping, ILog logger) : DataContext(options, logger, mapping);