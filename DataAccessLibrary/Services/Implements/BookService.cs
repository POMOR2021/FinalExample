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
        public async Task AddAsync(Book book) // Метод добаления книги
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Метод удаления книги
        {
            var book = await _context.Books.FindAsync(id);
            if(book is not null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync() // Метод получения книг
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id) // Метод получения книг по номеру
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task UpdateAsync(int id,Book updatedBook) // Метод обновления книги
        {
            var book = _context.Books.Find(id);
            if(book is not null)
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
