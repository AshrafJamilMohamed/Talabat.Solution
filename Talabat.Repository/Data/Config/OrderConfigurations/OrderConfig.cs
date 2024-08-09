using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregation;


namespace Talabat.Repository.Data.Config.OrderConfigurations
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, A => A.WithOwner());

            builder.Property(O => O.Status).HasConversion(
                (O) => O.ToString(), // In Database
                (O) =>(OrderStatus) Enum.Parse(typeof(OrderStatus), O) // When it returns to the App
                );

            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(O=>O.Items)
                .WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
