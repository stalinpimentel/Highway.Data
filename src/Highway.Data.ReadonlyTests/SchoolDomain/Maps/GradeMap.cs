using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Highway.Data.ReadonlyTests;

public sealed class GradeMap : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grade");
        builder.HasKey(x => x.GradeId);
    }
}