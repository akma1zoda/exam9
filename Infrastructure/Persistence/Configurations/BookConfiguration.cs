using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Price).HasColumnType("decimal(18,2)");
        builder.Property(b => b.PublishedYear).IsRequired();

        builder.HasOne(b => b.Category)
               .WithMany(c => c.Books)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}