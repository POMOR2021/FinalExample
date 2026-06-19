using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user) // Метод добавления пользователя
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Метод удаления пользователя
        {
            var user = await _context.Users.FindAsync(id);
            if(user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() // Метод получения пользователей
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUsersByIdAsync(int id) // Метод добавления пользователя по номеру
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(int id, User updatedUser) // Метод обновления пользователя
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                user.Login = updatedUser.Login;
                user.Role = updatedUser.Role;
                user.FullName = updatedUser.FullName;
                user.Password = updatedUser.Password;
                await _context.SaveChangesAsync();
            }
        }
    }
}
