using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using ShopBridge.Data.Entities;
using ShopBridge.Data.Extensions;
using ShopBridge.Data.Interfaces;
using System.Threading.Tasks;

namespace ShopBridge.Data.DbContexts
{
    public class ShopBridgeDbContext: DbContext, IShopBridgeDbContext
    {
        private readonly ConfigurationStoreOptions _storeOptions;
        public ShopBridgeDbContext() : this("server=(local);database=shopbridgedb;trusted_connection=yes;".StrToContextOptions<ShopBridgeDbContext>(),
            new ConfigurationStoreOptions())
        {
        }

        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options, ConfigurationStoreOptions storeOptions)
            : base(options)
        {
            _storeOptions = storeOptions;
        }
        public DbSet<ItemEntity> Items { get; set; }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureShopBridgeContext();
        }
    }
}
