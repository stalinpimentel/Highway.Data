using Highway.Data.Tests.TestDomain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public sealed class BazMap : IEntityTypeConfiguration<Baz>
{
    public void Configure(EntityTypeBuilder<Baz> builder)
    {
        builder.ToTable("Bazs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired(false);
        builder.HasMany(x => x.Quxes).WithOne();
    }
}