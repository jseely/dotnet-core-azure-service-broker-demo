using System;
using Microsoft.EntityFrameworkCore;
using product_inventory_service.Models;

namespace product_inventory_service.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) {}

        public DbSet<InventoryRecord> Inventory {get; set;}
    }
}