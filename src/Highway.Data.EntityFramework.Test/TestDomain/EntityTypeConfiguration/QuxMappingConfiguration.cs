using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public class QuxMappingConfiguration : BaseMappingConfiguration
{
    public override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuxMap());
        base.ConfigureModelBuilder(modelBuilder);
    }
}