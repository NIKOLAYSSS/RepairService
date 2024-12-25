using Npgsql;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepairService
{
    public partial class Form1 : Form
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly string _userRole;
        //string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";
        public Form1(string userRole)
        {
            InitializeComponent();
            ConfigureAccessByRole(); 
            // Инициализация DatabaseHelper с вашей строкой подключения
            string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";
            _dbHelper = new DatabaseHelper(connectionString);
            _userRole = userRole;
        }
        private void ConfigureAccessByRole()
        {
            // Отключаем или скрываем элементы интерфейса в зависимости от роли
            if (_userRole == "Admin")
            {
                button3.Enabled = true;
                btnGetStatistics.Enabled = true;
            }
            else if (_userRole == "User")
            {
                button3.Enabled = true;
                btnGetStatistics.Enabled = true;

            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;  // txtSearch - это TextBox для поиска
            var filteredRequests = _dbHelper.SearchRequests(searchTerm);
            dataGridViewRequests.DataSource = filteredRequests;
        }
        private void btnGetStatistics_Click(object sender, EventArgs e)
        {
            var statisticsForm = new StatisticsForm();
            statisticsForm.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRequest();
        }
        private void DeleteRequest()
        {
            // Получаем ID выбранной заявки
            if (dataGridViewRequests.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRequests.SelectedRows[0];
                Guid requestId = (Guid)selectedRow.Cells["RequestId"].Value; // Предполагаем, что в DataGridView есть столбец "RequestId"

                bool isDeleted = _dbHelper.DeleteRequest(requestId);
                if (isDeleted)
                {
                    MessageBox.Show("Заявка успешно удалена.");
                    // Обновляем DataGridView
                    LoadRequests();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении заявки.");
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления.");
            }
        }

        private void LoadRequests()
        {
            try
            {
                // Получение списка заявок из базы данных
                List<Request> requests = _dbHelper.GetRequests();

                // Установка источника данных для DataGridView
                dataGridViewRequests.DataSource = requests;

                // Автоматическое подстраивание колонок
                dataGridViewRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Обновление данных при нажатии кнопки "Обновить"
            LoadRequests();
        }
        private void btnOpenAddRequest(object sender, EventArgs e)
        {
            var addRequestForm = new AddRequestForm(_dbHelper);
            addRequestForm.ShowDialog();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите заявку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем первую выбранную строку
            var selectedRow = dataGridViewRequests.SelectedRows[0];

            // Извлекаем ID заявки из первой ячейки строки
            var requestId = (Guid)selectedRow.Cells["RequestId"].Value;

            // Открываем форму для редактирования с передачей ID заявки
            var editForm = new EditRequestForm(_dbHelper, requestId);
            editForm.ShowDialog();

            // Перезагружаем список заявок после редактирования
            LoadRequests();
        }
        private void BtnGenerateQRCode_Click(object sender, EventArgs e)
        {
            // Получаем данные из текстового поля
            string qrCodeText = "https://docs.google.com/forms/d/e/1FAIpQLSfkJf4oLCYcKbQggFu97aT6VplRHjBeAAj23LbdNANcQoncPw/viewform?usp=dialog";

            if (string.IsNullOrEmpty(qrCodeText))
            {
                MessageBox.Show("Введите данные для генерации QR-кода.");
                return;
            }

            try
            {
                // Генерация QR-кода с помощью QRCoder
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q); // Создаем данные QR

                // Генерация QR-кода как изображение
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(3); // 20 - это размер пикселей QR-кода

                // Отображаем QR-код в PictureBox
                pictureBoxQRCode.Image = qrCodeImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации QR-кода: " + ex.Message);
            }
        }

    }
}