using System;
using Microsoft.EntityFrameworkCore;
using product_inventory_service.Models;

namespace product_inventory_service.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InventoryRecord>()
                .HasKey(r => r.ID);
        }

        public DbSet<InventoryRecord> Inventory {get; set;}
    }
}