using Highway.Data.Tests.TestDomain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Highway.Data.EntityFramework.Test.TestDomain;

public sealed class QuxMap : IEntityTypeConfiguration<Qux>
{
    public void Configure(EntityTypeBuilder<Qux> builder)
    {
        builder.ToTable("Quxs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired(false);
    }
}