using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using product.Domain.Models;

namespace product.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("id");
        
        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");
        
        builder
            .Property(p=>p.Price)
            .HasPrecision(18, 2)
            .IsRequired()
            .HasColumnName("price");
        
        builder
            .Property(p => p.Quantity)
            .IsRequired()
            .HasColumnName("quantity");
        
        builder
            .Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at");;
    }
}