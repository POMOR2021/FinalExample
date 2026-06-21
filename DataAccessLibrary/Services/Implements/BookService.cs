using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Services.Implements
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод добаления книги
        /// </summary>
        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Метод удаления книги
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is not null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод получения книг
        /// </summary>
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        /// <summary>
        /// Метод получения книг по номеру
        /// </summary>
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        /// <summary>
        /// Метод обновления книги
        /// </summary>
        public async Task UpdateAsync(int id, Book updatedBook)
        {
            var book = _context.Books.Find(id);
            if (book is not null)
            {
                book.Author = updatedBook.Author;
                book.SKU = updatedBook.SKU;
                book.Quantity = updatedBook.Quantity;
                book.Category = updatedBook.Category;
                book.Description = updatedBook.Description;
                book.Manufacturer = updatedBook.Manufacturer;
                book.Unit = updatedBook.Unit;
                book.Sale = updatedBook.Sale;
                book.Name = updatedBook.Name;
                book.Photo = updatedBook.Photo;
                await _context.SaveChangesAsync();
            }
        }
    }
}