using DataAccessLibrary.Models;
using DataAccessLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get() //Получение всех заказов
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера при получении заказов.");
            }
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id) //Получение заказа по номеру
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order is null)
                {
                    return NotFound($"Заказ с id {id} не найден.");
                }
                return Ok(order);
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при получении заказа с id {id}.");
            }
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value) //Добавление заказа
        {
            try
            {
                await _orderService.AddAsync(value);
                return Created();
            }
            catch
            {
                return StatusCode(500, "Ошибка сервера при создании заказа.");
            }
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order value) //Обновление заказа
        {

            try
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder is null)
                {
                    return NotFound($"Заказ с id {id} не найден.");
                }

                await _orderService.UpdateAsync(id, value);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при обновлении заказа с id {id}.");
            }
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) //Удаление заказа
        {
            try
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder is null)
                {
                    return NotFound($"Заказ с id {id} не найден.");
                }

                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при удалении заказа с id {id}.");
            }
        }
    }
}
