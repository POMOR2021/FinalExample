using DataAccessLibrary.Models;
using System.Windows;

namespace FinalClentApp
{
    /// <summary>
    /// Класс окна авторизации
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки "Войти"
        /// </summary>
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = Password.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля", "Внимание", MessageBoxButton.OK);
                return;
            }

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
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK);
                }
            }
        }
    }
}