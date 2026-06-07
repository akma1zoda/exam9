using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
{
    public void Configure(EntityTypeBuilder<Borrow> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.BorrowDate).IsRequired();
        
        builder.HasOne(b => b.Book)
               .WithMany(bk => bk.Borrows)
               .HasForeignKey(b => b.BookId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Member)
               .WithMany(m => m.Borrows)
               .HasForeignKey(b => b.MemberId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}