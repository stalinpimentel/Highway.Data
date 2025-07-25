using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class BazMappingConfiguration : BaseMappingConfiguration
{
    public override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BazMap());
        base.ConfigureModelBuilder(modelBuilder);
    }
}