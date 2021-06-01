using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Data
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly InventoryContext _context;

        public InventoryRepo(InventoryContext context)
        {
            _context = context;
        }

        public async Task CreateInventoryItemAsync(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
            {
                throw new ArgumentNullException(nameof(inventoryItem));
            }
            _context.InventoryItems.Add(inventoryItem);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteInventoryItemAsync(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
            {
                throw new ArgumentNullException(nameof(inventoryItem));
            }
            _context.InventoryItems.Remove(inventoryItem);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }


        public async Task<IEnumerable<InventoryItem>> GetAllInventoriesAsync()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryItemByIdAsync(int id)
        {
            return await _context.InventoryItems.FirstOrDefaultAsync(item => item.Id == id);
        }


        public async Task UpdateInventoryItemAsync(InventoryItem inventoryItem)
        {
            await _context.SaveChangesAsync();
        }
    }
}