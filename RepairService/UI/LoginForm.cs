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
using RepairService.MODELS;
using RepairService.DAL;


namespace RepairService.UI
{
    public partial class LoginForm : Form
    {
        private readonly IUserService _userService;
        private string _currentUserRole;

        public LoginForm(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

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
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                _userService.RegisterUser(username, passwordHash);
                MessageBox.Show("Регистрация успешна!");
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                MessageBox.Show("Такой пользователь уже существует.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

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
                string storedHash = _userService.GetPasswordHash(username);

                if (!string.IsNullOrEmpty(storedHash))
                {
                    if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        _currentUserRole = _userService.GetUserRole(username);
                        MessageBox.Show($"Авторизация успешна! Ваша роль: {_currentUserRole}");
                        Form1 mainForm = new Form1(new DatabaseHelper("Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=db_repair_service"), _currentUserRole);
                        mainForm.Show();
                        this.Hide();
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
