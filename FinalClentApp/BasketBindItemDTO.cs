using DataAccessLibrary.Models;

namespace FinalClentApp
{
    /// <summary>
    /// Класс-обертка для склеивания двух сущностей
    /// </summary>
    public class BasketBindItemDTO
    {
        public Book Book { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}