using System.Collections.Generic;
using System.Threading.Tasks;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Data
{
    public interface IInventoryRepo
    {
        Task<IEnumerable<InventoryItem>> GetAllInventoriesAsync();
        Task<InventoryItem> GetInventoryItemByIdAsync(int id);
        Task CreateInventoryItemAsync(InventoryItem inventoryItem);
        Task UpdateInventoryItemAsync(InventoryItem inventoryItem);
        Task DeleteInventoryItemAsync(InventoryItem inventoryItem);

    }
}