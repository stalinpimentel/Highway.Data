using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.ReadonlyTests;

public class SchoolDomain : IDomain
{
    public string ConnectionString { get; } = Configuration.Instance.TestDatabaseConnectionString;

    public DbContextOptions Options
    {
        get
        {
            var builder = new DbContextOptionsBuilder()
                .UseSqlServer(ConnectionString, opts => opts.EnableRetryOnFailure());

            return ContextOptionsConfigurator is not null
                ? ContextOptionsConfigurator.ConfigureContextOptions(builder).Options
                : builder.Options;
        }
    }

    public IContextOptionsConfigurator? ContextOptionsConfigurator { get; } = EmptyContextOptionsConfigurator.Instance;

    public List<IInterceptor> Events { get; } = new();

    public IMappingConfiguration Mappings { get; } = new SchoolMapping();
}