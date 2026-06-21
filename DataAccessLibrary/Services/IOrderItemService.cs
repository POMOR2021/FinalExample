using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services
{
    /// <summary>
    ///Интерфейс для реализации сервиса элементов книг
    /// </summary>
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<OrderItem> GetOrderItemsByIdAsync(int id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(int id, OrderItem orderItem);
        Task DeleteAsync(int id);
    }
}
