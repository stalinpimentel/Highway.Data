using Highway.Data.Tests.TestDomain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public sealed class FooMap : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        builder.ToTable("Foos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired(false);
    }
}