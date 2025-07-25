using Highway.Data.Tests.TestDomain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public sealed class BarMap : IEntityTypeConfiguration<Bar>
{
    public void Configure(EntityTypeBuilder<Bar> builder)
    {
        builder.ToTable("Bars");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired(false);
    }
}