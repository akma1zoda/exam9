using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(200);
        builder.HasIndex(m => m.Email).IsUnique();
    }
}