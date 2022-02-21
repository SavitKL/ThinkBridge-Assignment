using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShopBridge.Data.Entities;
using System.Linq;

namespace ShopBridge.Data.Extensions
{
    public static class ShopBridgeModelBuilderExtensions
    {
        public static void ConfigureShopBridgeContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntity>(item =>
            {
                item.ToTable("Items");
                item.HasKey(i => i.Id);
                item.HasIndex(i =>  i.Name).IsUnique();
                item.Property(i => i.Name).IsRequired();
                item.Property(i => i.Price).IsRequired();
                item.Property(i => i.Quantity).IsRequired();

            });
        }
    }
}
