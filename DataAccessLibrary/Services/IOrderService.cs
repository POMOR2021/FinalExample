using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services
{
    /// <summary>
    /// Интерфейс для реализации сервиса заказов
    /// </summary>
    public interface IOrderService 
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(int id, Order order);
        Task DeleteAsync(int id);
    }
}
