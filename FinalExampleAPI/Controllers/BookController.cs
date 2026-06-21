using DataAccessLibrary.Models;
using DataAccessLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/<BookController> 
        [HttpGet]
        /// <summary>
        /// Получение всех книг 
        /// </summary>
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера при получении данных.");
            }
        }

        // GET api/<BookController>/5 
        [HttpGet("{id}")]
        /// <summary>
        /// Получение книги по номеру
        /// </summary>
        public async Task<ActionResult<Book>> Get(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound($"Книга с id {id} не найдена.");
                }
                return Ok(book);
            }
            catch
            {
                return StatusCode(500, $"Ошибка при получении книги с id {id}.");
            }
        }

        // POST api/<BookController> 
        [HttpPost]
        /// <summary>
        /// Добавление книги
        /// </summary>
        public async Task<IActionResult> Post([FromBody] Book value)
        {
            try
            {
                await _bookService.AddAsync(value);
                return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
            }
            catch
            {
                return StatusCode(500, "Ошибка при добавлении новой книги.");
            }
        }

        // PUT api/<BookController>/5 
        [HttpPut("{id}")]
        /// <summary>
        /// Обновление книги
        /// </summary>
        public async Task<IActionResult> Put(int id, [FromBody] Book value)
        {
            try
            {
                var existingBook = await _bookService.GetBookByIdAsync(id);
                if (existingBook is null)
                {
                    return NotFound($"Книга с id {id} не найдена.");
                }

                await _bookService.UpdateAsync(id, value);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка при обновлении книги с id {id}.");
            }
        }

        // DELETE api/<BookController>/5 
        [HttpDelete("{id}")]
        /// <summary>
        /// Удаление книги
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingBook = await _bookService.GetBookByIdAsync(id);
                if (existingBook is null)
                {
                    return NotFound($"Книга с id {id} не найдена.");
                }

                await _bookService.DeleteAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка при удалении книги с id {id}.");
            }
        }
    }
}