using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using product_inventory_service.Models;
using product_inventory_service.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace product_inventory_service.Controllers
{
    public class InventoryController : Controller
    {
        private InventoryContext _context;
        public InventoryController(IServiceProvider serviceProvider)
        {
            _context = new InventoryContext(serviceProvider.GetRequiredService<DbContextOptions<InventoryContext>>());
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        public IActionResult SetProductInventory(string id, [FromBody]int quantity)
        {
            var record = new InventoryRecord{ID = id, Quantity = quantity};
            if (_context.Inventory.Any(r => r.ID == record.ID))
            {
                _context.Inventory.Update(record);
            } else 
            {
                _context.Inventory.Add(record);
            }
            _context.SaveChanges();
            return Json(record);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public IActionResult GetProductInventory(string id)
        {
            try
            {
                return Json(_context.Inventory.First(r => r.ID == id));
            }
            catch
            {
                return StatusCode(404);
            }
        }
    }
}
