using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTDStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
namespace OTDStore.Data.Configurations
{
    public class ProductInBrandConfiguration : IEntityTypeConfiguration<ProductInBrand>
    {
        public void Configure(EntityTypeBuilder<ProductInBrand> builder)
        {
            builder.HasKey(t => new { t.BrandId, t.ProductId });

            builder.ToTable("ProductInBrands");

            builder.HasOne(t => t.Product).WithMany(pc => pc.ProductInBrands)
                .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(t => t.Brand).WithMany(pc => pc.ProductInBrands)
              .HasForeignKey(pc => pc.BrandId);
        }
    }
}
