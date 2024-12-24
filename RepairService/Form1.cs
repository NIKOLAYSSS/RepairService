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

namespace RepairService
{
    public partial class Form1 : Form
    {
        private readonly DatabaseHelper _dbHelper;
        string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";
        public Form1()
        {
            InitializeComponent();
            // Инициализация DatabaseHelper с вашей строкой подключения
            string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";
            _dbHelper = new DatabaseHelper(connectionString);

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

                // Подключаемся к базе данных и выполняем запрос на удаление
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM requests WHERE requestid = @requestid";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@requestid", requestId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
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


    }
}