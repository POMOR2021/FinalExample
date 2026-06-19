using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLibrary.Models
{
    [Table("FinalBook")]
    public class Book // Модель книг
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Author { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public int Sale { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
    }
}
