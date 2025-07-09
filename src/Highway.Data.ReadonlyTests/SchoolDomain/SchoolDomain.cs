using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.ReadonlyTests;

public class SchoolDomain : IDomain
{
    public string ConnectionString { get; } = Configuration.Instance.TestDatabaseConnectionString;

    public DbContextOptions Options { get; }

    public IContextConfiguration Context { get; } = new DefaultContextConfiguration();

    public List<IInterceptor> Events { get; } = new();

    public IMappingConfiguration Mappings { get; } = new SchoolMapping();

    public SchoolDomain()
    {
        Options = new DbContextOptionsBuilder()
                  .UseSqlServer(ConnectionString, opts => opts.EnableRetryOnFailure())
            .Options;
    }
}