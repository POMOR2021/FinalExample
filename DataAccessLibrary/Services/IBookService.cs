using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services
{
    /// <summary>
    /// Интерфейс для реализации сервиса книг
    /// </summary>
    public interface IBookService 
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(int id, Book book);
        Task DeleteAsync(int id);
    }
}
