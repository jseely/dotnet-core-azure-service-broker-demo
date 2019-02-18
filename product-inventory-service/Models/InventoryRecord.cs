using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace product_inventory_service.Models
{
    [Table("inventory")]
    public class InventoryRecord
    {
        [Key]
        public string ID {get;set;}
        public int Quantity {get;set;}
    }
}