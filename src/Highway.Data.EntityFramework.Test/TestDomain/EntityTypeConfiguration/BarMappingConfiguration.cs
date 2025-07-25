using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class BarMappingConfiguration : BaseMappingConfiguration
{
    public override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BarMap());
        base.ConfigureModelBuilder(modelBuilder);
    }
}