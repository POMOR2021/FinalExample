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
        public async Task AddAsync(Order order)  // Метод добавления книги
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Метод удаления книги
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync() // Метод получения книг
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id) // Метод получения книги по номеру
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(int id, Order updatedOrder) // Метод обновления книги
        {
            var order = await _context.Orders.FindAsync(id);
            if(order is not null)
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