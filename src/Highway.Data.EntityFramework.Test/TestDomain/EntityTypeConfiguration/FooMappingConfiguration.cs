using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class FooMappingConfiguration : BaseMappingConfiguration
{
    public bool Called { get; set; }

    public override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        Called = true;
        modelBuilder.ApplyConfiguration(new FooMap());
        base.ConfigureModelBuilder(modelBuilder);
    }
}