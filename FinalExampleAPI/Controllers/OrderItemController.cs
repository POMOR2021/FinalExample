using DataAccessLibrary.Models;
using DataAccessLibrary.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderService)
        {
            _orderItemService = orderService;
        }

        // GET: api/<OrderController>
        [HttpGet]
        /// <summary>
        /// Получение всех элементов заказа
        /// </summary>
        public async Task<ActionResult<IEnumerable<OrderItem>>> Get()
        {
            var orders = await _orderItemService.GetAllOrderItemsAsync();
            return Ok(orders);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        /// <summary>
        /// Получение элемента заказа по номеру
        /// </summary>
        public async Task<ActionResult<Order>> Get(int id)
        {
            try
            {
                var order = await _orderItemService.GetOrderItemsByIdAsync(id);
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
        /// <summary>
        /// Добавление элемента заказа
        /// </summary>
        public async Task<IActionResult> Post([FromBody] OrderItem value)
        {
            try
            {
                await _orderItemService.AddAsync(value);
                return Created();
            }
            catch
            {
                return StatusCode(500, "Ошибка сервера при создании заказа.");
            }
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        /// <summary>
        /// Обновление элемента заказа
        /// </summary>
        public async Task<IActionResult> Put(int id, [FromBody] OrderItem value)
        {

            try
            {
                var existingOrder = await _orderItemService.GetOrderItemsByIdAsync(id);
                if (existingOrder is null)
                {
                    return NotFound($"Заказ с id {id} не найден.");
                }

                await _orderItemService.UpdateAsync(id, value);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при обновлении заказа с id {id}.");
            }
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        /// <summary>
        /// Удаление элемента заказа
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingOrder = await _orderItemService.GetOrderItemsByIdAsync(id);
                if (existingOrder is null)
                {
                    return NotFound($"Заказ с id {id} не найден.");
                }

                await _orderItemService.DeleteAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, $"Ошибка сервера при удалении заказа с id {id}.");
            }
        }
    }
}