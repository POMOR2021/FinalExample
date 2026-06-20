using DataAccessLibrary.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalClentApp
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public ObservableCollection<Book> Books { get; set; }

        public static readonly DependencyProperty UserRoleProperty =
            DependencyProperty.Register("UserRole", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public string UserRole
        {
            get { return (string)GetValue(UserRoleProperty); }
            set { SetValue(UserRoleProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            Books = new ObservableCollection<Book>();
            DataContext = this;

            Loaded += MainWindow_Loaded;

            UpdateUI();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await FetchDataFromWebApiAsync();
        }

        private async Task FetchDataFromWebApiAsync()
        {
            string url = "https://localhost:7123/api/Book";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiProducts = JsonSerializer.Deserialize<List<Book>>(json, options);

                    Books.Clear();
                    if (apiProducts != null)
                    {
                        foreach (var prod in apiProducts)
                        {
                            Books.Add(prod);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Сервер вернул ошибку: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к бэкенду: {ex.Message}");
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new();
            loginWindow.Owner = this;
            if (loginWindow.ShowDialog() == true)
            {
                UpdateUI();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FullName = string.Empty;
            Properties.Settings.Default.Role = string.Empty;
            Properties.Settings.Default.IsLogged = false;
            Properties.Settings.Default.Save();

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (Properties.Settings.Default.IsLogged)
            {
                TxtUserFIO.Text = Properties.Settings.Default.FullName;
                SignInButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;

                UserRole = Properties.Settings.Default.Role;
            }
            else
            {
                TxtUserFIO.Text = "Гость";
                SignInButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Collapsed;

                UserRole = string.Empty;
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow();
            orderWindow.ShowDialog();
        }
    }
}