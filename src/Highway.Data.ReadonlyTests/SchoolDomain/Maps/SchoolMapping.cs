using Microsoft.EntityFrameworkCore;

namespace Highway.Data.ReadonlyTests;

public class SchoolMapping : IMappingConfiguration
{
    public void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GradeMap());
        modelBuilder.ApplyConfiguration(new StudentMap());
    }
}