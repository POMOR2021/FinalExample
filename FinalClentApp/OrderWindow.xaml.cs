using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinalClentApp
{
    public partial class OrderWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string orderApiUrl = "https://localhost:7123/api/Order";

        public OrderWindow()
        {
            InitializeComponent();
            RefreshOrderList();
            DisplayClientInfo();
        }

        /// <summary>
        /// Метод отображения статуса авторизации клиента
        /// </summary>
        private void DisplayClientInfo()
        {
            if (Properties.Settings.Default.IsLogged)
            {
                ClientInfoTextBlock.Text = $"Клиент: {Properties.Settings.Default.FullName}";
            }
            else
            {
                ClientInfoTextBlock.Text = "Клиент: Гость (Не авторизован)";
            }
        }

        /// <summary>
        /// Метод обновления корзины
        /// </summary>
        private void RefreshOrderList()
        {
            ShoppingBasket.Items.RemoveAll(i => i.OrderItem.Quantity <= 0);

            OrderItemsListBox.ItemsSource = null;
            OrderItemsListBox.ItemsSource = ShoppingBasket.Items;

            decimal total = ShoppingBasket.Items.Sum(i => i.OrderItem.Price * i.OrderItem.Quantity);
            TotalSumTextBlock.Text = $"Итого: {total} руб.";
        }

        /// <summary>
        /// Обработчик тектовго поля с количеством
        /// </summary>
        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is BasketBindItemDTO item)
            {
                if (int.TryParse(textBox.Text, out int newQuantity))
                {
                    item.OrderItem.Quantity = newQuantity;

                    if (newQuantity <= 0)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => RefreshOrderList()));
                        return;
                    }
                }

                decimal total = ShoppingBasket.Items.Sum(i => i.OrderItem.Price * i.OrderItem.Quantity);
                TotalSumTextBlock.Text = $"Итого: {total} руб.";
            }
        }

        /// <summary>
        /// Обработчик кнопки "Удалить"
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BasketBindItemDTO item)
            {
                ShoppingBasket.Items.Remove(item);
                RefreshOrderList();
            }
        }

        /// <summary>
        /// Обработчик кнопки "Оформить заказ"
        /// </summary>
        private async void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShoppingBasket.Items.Count == 0)
            {
                MessageBox.Show("Оформить пустой заказ нельзя");
                return;
            }

            try
            {
                Order newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(3),
                    Status = "Новый",
                    PickupCode = new Random().Next(100, 999).ToString(),
                    ClientName = Properties.Settings.Default.IsLogged ? Properties.Settings.Default.FullName : ""
                };

                HttpResponseMessage response = await client.PostAsJsonAsync(orderApiUrl, newOrder);

                if (response.IsSuccessStatusCode)
                {
                    int actualOrderId = 0;

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(responseContent) && responseContent.Trim().StartsWith("{"))
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        Order createdOrder = JsonSerializer.Deserialize<Order>(responseContent, options);
                        if (createdOrder != null)
                        {
                            actualOrderId = createdOrder.Id;
                        }
                    }

                    if (actualOrderId == 0)
                    {
                        HttpResponseMessage getOrdersResponse = await client.GetAsync(orderApiUrl);
                        if (getOrdersResponse.IsSuccessStatusCode)
                        {
                            string json = await getOrdersResponse.Content.ReadAsStringAsync();
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var allOrders = JsonSerializer.Deserialize<List<Order>>(json, options) ?? new List<Order>();
                            if (allOrders.Count > 0)
                            {
                                actualOrderId = allOrders.Max(o => o.Id);
                            }
                        }
                    }

                    foreach (var bindItem in ShoppingBasket.Items)
                    {
                        OrderItem dbItem = bindItem.OrderItem;
                        dbItem.OrderId = actualOrderId;
                        dbItem.Id = 0;

                        await client.PostAsJsonAsync("https://localhost:7123/api/OrderItem", dbItem);
                    }

                    MessageBox.Show($"Заказ №{actualOrderId} успешно оформлен!\nКод получения: {newOrder.PickupCode}",
                                    "Успех", MessageBoxButton.OK);

                    ShoppingBasket.Items.Clear();
                    Close();
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}