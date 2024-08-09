using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Repository.Data.Config.OrderConfigurations
{
    internal class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(O => O.Product, P => P.WithOwner());

            builder.Property(I => I.Price).HasColumnType("decimal(18,2)");
        }
    }
}
