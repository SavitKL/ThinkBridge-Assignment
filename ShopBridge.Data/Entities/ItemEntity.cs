using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Data.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
