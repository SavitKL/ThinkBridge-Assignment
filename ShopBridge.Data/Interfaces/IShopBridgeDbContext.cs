using Microsoft.EntityFrameworkCore;
using ShopBridge.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ShopBridge.Data.Interfaces
{
    public interface IShopBridgeDbContext:IDisposable
    {
        public DbSet<ItemEntity> Items { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
