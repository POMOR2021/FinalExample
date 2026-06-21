using DataAccessLibrary.Models;

namespace FinalClentApp
{
    /// <summary>
    /// Класс корзины товаров 
    /// </summary>
    public static class ShoppingBasket 
    {
        public static List<BasketBindItemDTO> Items { get; set; } = new();

        public static void AddBook(Book book)
        {
            var existingItem = Items.Find(i => i.Book.Id == book.Id);

            if (existingItem != null)
            {
                existingItem.OrderItem.Quantity++;
            }
            else
            {
                var newOrderItem = new OrderItem
                {
                    ProductId = book.Id,
                    Quantity = 1,
                    Price = book.Sale
                };

                Items.Add(new BasketBindItemDTO
                {
                    Book = book,
                    OrderItem = newOrderItem
                });
            }
        }
    }
}

