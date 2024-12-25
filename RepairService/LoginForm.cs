using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;


namespace RepairService
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper _databaseManager;
        private string _currentUserRole;

        public LoginForm()
        {
            string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";
            _databaseManager = new DatabaseHelper(connectionString);
            InitializeComponent();
        }

        // Обработчик кнопки регистрации
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Имя пользователя и пароль не могут быть пустыми.");
                return;
            }

            try
            {
                // Хэшируем пароль
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                // Регистрируем пользователя через DatabaseManager
                _databaseManager.RegisterUser(username, passwordHash);
                MessageBox.Show("Регистрация успешна!");
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Проверка уникальности имени
            {
                MessageBox.Show("Такой пользователь уже существует.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        // Обработчик кнопки входа
        // Обработчик кнопки входа
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Имя пользователя и пароль не могут быть пустыми.");
                return;
            }

            try
            {
                // Получаем хэш пароля через DatabaseManager
                string storedHash = _databaseManager.GetPasswordHash(username);

                if (!string.IsNullOrEmpty(storedHash))
                {
                    // Проверяем пароль
                    if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        // Получаем роль пользователя
                        _currentUserRole = _databaseManager.GetUserRole(username);
                        MessageBox.Show($"Авторизация успешна! Ваша роль: {_currentUserRole}");
                        Form1 mainForm = new Form1(_currentUserRole); // Создаём объект главной формы
                        mainForm.Show(); // Открываем главную форму
                        this.Hide(); // Скрываем текущую форму
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль.");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
