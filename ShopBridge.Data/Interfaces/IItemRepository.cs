using ShopBridge.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Data.Interfaces
{
    public interface IItemRepository: IRepository
    {
        Task<List<ItemEntity>> GetAllItemAsync(string searchText);
        Task<ItemEntity> GetItemAsync(string itemId);
        Task<ItemEntity> UpdateItemAsync(ItemEntity item);
        Task<ItemEntity> AddItemAsync(ItemEntity item);
        Task<bool> DeleteItemAsync(string itemId);
        Task<bool> IsItemExistsAsync(string itemId);
    }
}
