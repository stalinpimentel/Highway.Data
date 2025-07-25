using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain;

public class SqlLiteDomainMappings : IMappingConfiguration
{
    public void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(e => e.Id);
    }
}