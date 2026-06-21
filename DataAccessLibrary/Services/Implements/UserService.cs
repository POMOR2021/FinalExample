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

        /// <summary>
        /// Метод добавления пользователя
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Метод удаления пользователя
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод получения пользователей
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Метод добавления пользователя по номеру
        /// </summary>
        public async Task<User> GetUsersByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Метод обновления пользователя
        /// </summary>
        public async Task UpdateAsync(int id, User updatedUser)
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