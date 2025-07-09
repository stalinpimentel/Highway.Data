using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain;

public class SqlLiteDomain : IDomain
{
    public string ConnectionString => @"Data Source=:memory:";
    
    public DbContextOptions Options { get; } 

    public IContextConfiguration Context => new SqlLiteDomainContextConfiguration();

    public List<IInterceptor> Events => new List<IInterceptor>();

    public IMappingConfiguration Mappings => new SqlLiteDomainMappings();

    public SqlLiteDomain()
    {
        Options = new DbContextOptionsBuilder()
            .UseSqlite(ConnectionString)
            .Options;
    }
}