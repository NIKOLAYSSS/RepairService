using Npgsql;
using QRCoder;
using RepairService.MODELS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepairService.UI
{
    public partial class Form1 : Form
    {
        private readonly IRequestService _requestService;
        private readonly string _userRole;

        public Form1(IRequestService requestService, string userRole)
        {
            InitializeComponent();
            _requestService = requestService;
            _userRole = userRole;
            ConfigureAccessByRole();
        }

        private void ConfigureAccessByRole()
        {
            if (_userRole == "admin")
            {
                button3.Enabled = true;
                btnGetStatistics.Enabled = false;
            }
            else if (_userRole == "user")
            {
                button3.Enabled = false;
                btnGetStatistics.Enabled = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            var filteredRequests = _requestService.SearchRequests(searchTerm);
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
            if (dataGridViewRequests.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRequests.SelectedRows[0];
                Guid requestId = (Guid)selectedRow.Cells["RequestId"].Value;

                bool isDeleted = _requestService.DeleteRequest(requestId);
                if (isDeleted)
                {
                    MessageBox.Show("Заявка успешно удалена.");
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
                List<Request> requests = _requestService.GetRequests();
                dataGridViewRequests.DataSource = requests;
                dataGridViewRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRequests();
        }

        private void btnOpenAddRequest(object sender, EventArgs e)
        {
            var addRequestForm = new AddRequestForm(_requestService);
            addRequestForm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите заявку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewRequests.SelectedRows[0];
            var requestId = (Guid)selectedRow.Cells["RequestId"].Value;

            var editForm = new EditRequestForm(_requestService, requestId);
            editForm.ShowDialog();

            LoadRequests();
        }

        private void BtnGenerateQRCode_Click(object sender, EventArgs e)
        {
            string qrCodeText = "https://docs.google.com/forms/d/e/1FAIpQLSfkJf4oLCYcKbQggFu97aT6VplRHjBeAAj23LbdNANcQoncPw/viewform?usp=dialog";

            if (string.IsNullOrEmpty(qrCodeText))
            {
                MessageBox.Show("Введите данные для генерации QR-кода.");
                return;
            }

            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(3);
                pictureBoxQRCode.Image = qrCodeImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации QR-кода: " + ex.Message);
            }
        }
    }

}