using Microsoft.EntityFrameworkCore;
using ShopBridge.Data.DbContexts;
using ShopBridge.Data.Entities;
using ShopBridge.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data.Repository
{
    public class ItemRepository: Repository, IItemRepository
    {
        private readonly ShopBridgeDbContext _dbContext;
        public ItemRepository(ShopBridgeDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ItemEntity>> GetAllItemAsync(string searchText)
        {
            var query = _dbContext.Items
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(u => u.Name.ToLower().Contains(searchText));
            }

            query = query.OrderBy(u => u.Name);

            var Items = await query.ToListAsync();

            return new List<ItemEntity>(Items);
        }
        public async Task<ItemEntity> GetItemAsync(string name)
        {
            var Item = await _dbContext.Items
            .AsNoTracking()
            .Where(u => u.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
            return Item;
        }
        public async Task<ItemEntity> UpdateItemAsync(ItemEntity Item)
        {
            _dbContext.Entry(Item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Item;
        }
        public async Task<ItemEntity> AddItemAsync(ItemEntity Item)
        {
            await _dbContext.Items.AddAsync(Item);
            return Item;
        }
        public async Task<bool> DeleteItemAsync(string name)
        {
            var ItemTobeDeleted = await _dbContext.Items.Where(u => u.Name.ToLower() == name.ToLower())
             .FirstOrDefaultAsync();
            if (ItemTobeDeleted != null)
            {
                _dbContext.Items.Remove(ItemTobeDeleted);
                return true;
            }

            return false;
        }
        public Task<bool> IsItemExistsAsync(string name)
        {
            return _dbContext.Items?.AsNoTracking().AnyAsync(u => u.Name.ToLower() == name.ToLower());
        }
    }
}
