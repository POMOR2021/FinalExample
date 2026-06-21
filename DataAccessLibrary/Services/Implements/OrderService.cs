using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добавления заказа
        /// </summary>
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Метод удаления заказа
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод получения заказов
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        /// <summary>
        /// Метод получения заказа по номеру
        /// </summary>
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        /// <summary>
        /// Метод обновления заказа
        /// </summary>
        public async Task UpdateAsync(int id, Order updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                order.OrderDate = updatedOrder.OrderDate;
                order.DeliveryDate = updatedOrder.DeliveryDate;
                order.Status = updatedOrder.Status;
                order.ClientName = updatedOrder.ClientName;
                order.PickupCode = updatedOrder.PickupCode;
                await _context.SaveChangesAsync();
            }
        }
    }
}