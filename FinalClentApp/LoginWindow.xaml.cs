using DataAccessLibrary.Models;
using System;
using System.Linq; // Убедитесь, что этот using добавлен для работы FirstOrDefault
using System.Windows;

namespace FinalClentApp
{
    public partial class LoginWindow : Window
    {
        // Конструктор теперь пустой и ничего не требует
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = Password.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем контекст только на время выполнения запроса
            using (var context = new AppDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user is not null)
                {
                    Properties.Settings.Default.FullName = user.FullName;
                    Properties.Settings.Default.Role = user.Role;
                    Properties.Settings.Default.IsLogged = true;
                    Properties.Settings.Default.Save();

                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } // Здесь context автоматически закроется и уничтожится
        }
    }
}
