using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services
{
    /// <summary>
    /// Интерфейс для реализации сервиса пользователей
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUsersByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(int id, User user);
        Task DeleteAsync(int id);
    }
}
