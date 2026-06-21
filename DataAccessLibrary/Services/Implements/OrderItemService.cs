using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Services.Implements
{
    public class OrderItemService : IOrderItemService
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод доавбления элемента заказа
        /// </summary>
        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Метод удаления элемента заказа
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem is not null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод получения элементов заказа
        /// </summary>
        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        /// <summary>
        /// Метод получения элемента заказа по номеру
        /// </summary>
        public async Task<OrderItem> GetOrderItemsByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        /// <summary>
        /// Метод обновления элемента заказа
        /// </summary>
        public async Task UpdateAsync(int id, OrderItem updatedOrderItem)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem is not null)
            {
                orderItem.Price = updatedOrderItem.Price;
                orderItem.Quantity = updatedOrderItem.Quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}