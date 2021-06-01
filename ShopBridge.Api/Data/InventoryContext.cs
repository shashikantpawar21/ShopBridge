using Microsoft.EntityFrameworkCore;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> opt) : base(opt)
        {

        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
    }

}