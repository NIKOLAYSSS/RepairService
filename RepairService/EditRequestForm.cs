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
    public partial class EditRequestForm : Form
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly Guid _requestId;

        public EditRequestForm(DatabaseHelper dbHelper, Guid requestId)
        {
            _dbHelper = dbHelper;
            _requestId = requestId;
            InitializeComponent();
            LoadRequestData();
        }

        private void LoadRequestData()
        {
            try
            {
                // Получаем данные заявки из базы данных
                var request = _dbHelper.GetRequestById(_requestId);

                if (request == null)
                {
                    MessageBox.Show("Заявка не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Заполняем поля формы данными заявки
                txtDescription.Text = request.Description;
                cmbStatus.SelectedItem = request.Status;
                txtResponsible.Text = request.Responsible ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Обновляем данные заявки
                var updatedRequest = new Request
                {
                    RequestId = _requestId,
                    //RequestDate = DateTime.Now,
                    Description = txtDescription.Text.Trim(),
                    Status = cmbStatus.SelectedItem?.ToString(),
                    Responsible = txtResponsible.Text.Trim()
                };

                // Проверка обязательных полей
                if (string.IsNullOrEmpty(updatedRequest.Description) || string.IsNullOrEmpty(updatedRequest.Status))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сохранение изменений в базу данных
                _dbHelper.UpdateRequest(updatedRequest);

                MessageBox.Show("Заявка успешно обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }
}
