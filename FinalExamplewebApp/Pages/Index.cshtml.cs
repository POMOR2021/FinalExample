using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FinalExamplewebApp.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ProductsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Book> Products { get; set; }
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Метод получения данных с сервера 
        /// </summary>
        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7123/api/Book");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Products = JsonSerializer.Deserialize<IEnumerable<Book>>(json, options);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Произошла ошибка при обработке данных: {ex}.";
            }
        }

        /// <summary>
        /// Метод записи сообщения на вспомогательню страницу
        /// </summary>
        public IActionResult OnPostOrder(string title, string price)
        {
            TempData["OrderMessage"] = $"Товар \"{title}\" добавлен в заказ.";

            return RedirectToPage();
        }
    }
}