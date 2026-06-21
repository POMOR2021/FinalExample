using DataAccessLibrary.Models;
using DataAccessLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера при получении пользователей.");
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        /// <summary>
        /// Получение пользователя по номеру
        /// </summary>
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUsersByIdAsync(id);
                if (user is null)
                {
                    return NotFound($"Пользователь с id {id} не найден.");
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при получении пользователя с id {id}.");
            }
        }

        // POST api/<UserController>
        [HttpPost]
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        public async Task<IActionResult> Post([FromBody] User value)
        {
            try
            {
                await _userService.AddAsync(value);
                return Created();
            }
            catch
            {
                return StatusCode(500, "Ошибка сервера при создании пользователя.");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        /// <summary>
        /// Обновление пользователя
        /// </summary>
        public async Task<IActionResult> Put(int id, [FromBody] User value)
        {
            try
            {
                var existingUser = await _userService.GetUsersByIdAsync(id);
                if (existingUser is null)
                {
                    return NotFound($"Пользователь с id {id} не найден.");
                }

                await _userService.UpdateAsync(id, value);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при обновлении пользователя с id {id}.");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingUser = await _userService.GetUsersByIdAsync(id);
                if (existingUser is null)
                {
                    return NotFound($"Пользователь с id {id} не найден.");
                }

                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при удалении пользователя с id {id}.");
            }
        }
    }
}