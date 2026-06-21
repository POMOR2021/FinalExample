using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models
{
    [Table("FinalBook")]
    /// <summary>
    /// Модель книг
    /// </summary>
    public class Book
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
