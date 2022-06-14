using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTDStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.Color).IsRequired();

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.Memory).IsRequired();

            builder.Property(x => x.RAM).IsRequired();

            builder.Property(x => x.Price).IsRequired();

            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);
        }
    }
}
