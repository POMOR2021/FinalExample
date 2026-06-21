using DataAccessLibrary.Models;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace FinalClentApp
{
    /// <summary>
    /// Класс главного окна
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private List<Book> _allBooks = new();
        public List<Book> DisplayedBooks { get; set; } = new();

        public List<string> SortOptions { get; } = new()
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию",
            "Наименование по алфавиту",
            "Наименование с конца алфавита"
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
            UpdateUI();
        }

        /// <summary>
        /// Метод инициализации данных окна
        /// </summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string url = "https://localhost:7123/api/Book";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    _allBooks = JsonSerializer.Deserialize<List<Book>>(json, options) ?? new List<Book>();

                    UpdateManufacturersList();
                    ApplyFilterAndSort();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Метод обновления списка производителей
        /// </summary>
        private void UpdateManufacturersList()
        {
            ManufacturerComboBox.Items.Clear();
            ManufacturerComboBox.Items.Add("Все производители");

            var manufacturers = _allBooks
                .Select(b => b.Manufacturer)
                .Where(m => !string.IsNullOrEmpty(m))
                .Distinct()
                .OrderBy(m => m);

            foreach (var m in manufacturers)
            {
                ManufacturerComboBox.Items.Add(m);
            }
            ManufacturerComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Метод фильтров и сортировки
        /// </summary>
        private void ApplyFilterAndSort()
        {
            if (_allBooks == null) return;

            IEnumerable<Book> filtered = _allBooks;

            string searchText = SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
                filtered = filtered.Where(b => b.Name != null && b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            string selectedMan = ManufacturerComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedMan) && selectedMan != "Все производители")
                filtered = filtered.Where(b => b.Manufacturer == selectedMan);

            if (decimal.TryParse(MinPriceTextBox.Text, out decimal minPrice))
                filtered = filtered.Where(b => b.Sale >= minPrice);

            if (decimal.TryParse(MaxPriceTextBox.Text, out decimal maxPrice))
                filtered = filtered.Where(b => b.Sale <= maxPrice);

            switch (SortComboBox.SelectedIndex)
            {
                case 1:
                    filtered = filtered.OrderBy(b => b.Sale); break;
                case 2:
                    filtered = filtered.OrderByDescending(b => b.Sale); break;
                case 3:
                    filtered = filtered.OrderBy(b => b.Name); break;
                case 4:
                    filtered = filtered.OrderByDescending(b => b.Name); break;
            }

            DisplayedBooks = filtered.ToList();

            BooksListBox.ItemsSource = null;
            BooksListBox.ItemsSource = DisplayedBooks;

            TxtRecordsInfo.Text = $"Количество элементов: {DisplayedBooks.Count} из {_allBooks.Count}";
        }

        /// <summary>
        /// Обработчик текстового поля поиска
        /// </summary>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFilterAndSort();

        /// <summary>
        /// Обработчик выпадающего списка производителей
        /// </summary>
        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => ApplyFilterAndSort();

        /// <summary>
        /// Обработчик текстового поля минимальной цены
        /// </summary>
        private void MinPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFilterAndSort();

        /// <summary>
        /// Обработчик текстового поля максимальной цены
        /// </summary>
        private void MaxPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFilterAndSort();

        /// <summary>
        /// Обработчик выпадающего списка сортировки
        /// </summary>
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => ApplyFilterAndSort();

        /// <summary>
        /// Обработчик кнопки "Bойти"
        /// </summary>
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new() { Owner = this };
            if (loginWindow.ShowDialog() == true)
                UpdateUI();
        }

        /// <summary>
        /// Обработчик кнопки "Выйти"
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FullName = "";
            Properties.Settings.Default.Role = "";
            Properties.Settings.Default.IsLogged = false;
            Properties.Settings.Default.Save();

            UpdateUI();
        }

        /// <summary>
        /// Метод обновления UI в связи с авторизацией
        /// </summary>
        public void UpdateUI()
        {
            if (Properties.Settings.Default.IsLogged)
            {
                FullNameTextBlock.Text = Properties.Settings.Default.FullName;
                SignInButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;
            }
            else
            {
                FullNameTextBlock.Text = "Гость";
                SignInButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Collapsed;
            }

            if (BooksListBox != null)
                ApplyFilterAndSort();

            UpdateOrderButtonVisibility();
        }

        /// <summary>
        /// Обработчик кнопки "Заказать"
        /// </summary>
        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Book selectedBook)
            {
                ShoppingBasket.AddBook(selectedBook);

                UpdateOrderButtonVisibility();

                MessageBox.Show($"Книга \"{selectedBook.Name}\" добавлена в заказ.");
            }
        }

        /// <summary>
        /// Обработчик кнопки "Просмотр заказов"
        /// </summary>
        private void ViewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Owner = this;
            orderWindow.ShowDialog();

            UpdateOrderButtonVisibility();
        }

        /// <summary>
        /// Метод для обновления visibility кнопки
        /// </summary>
        private void UpdateOrderButtonVisibility()
        {
            if (ViewOrderButton == null)
                return;

            if (ShoppingBasket.Items != null && ShoppingBasket.Items.Count > 0)
                ViewOrderButton.Visibility = Visibility.Visible;
            else
                ViewOrderButton.Visibility = Visibility.Collapsed;
        }
    }
}