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
    public partial class EditRequestForm : Form
    {
        private readonly IRequestService _requestService;
        private readonly Guid _requestId;

        public EditRequestForm(IRequestService requestService, Guid requestId)
        {
            _requestService = requestService;
            _requestId = requestId;
            InitializeComponent();
            LoadRequestData();
        }

        private void LoadRequestData()
        {
            try
            {
                var request = _requestService.GetRequestById(_requestId);

                if (request == null)
                {
                    MessageBox.Show("Заявка не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

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
                var updatedRequest = new Request
                {
                    RequestId = _requestId,
                    Description = txtDescription.Text.Trim(),
                    Status = cmbStatus.SelectedItem?.ToString(),
                    Responsible = txtResponsible.Text.Trim()
                };

                if (string.IsNullOrEmpty(updatedRequest.Description) || string.IsNullOrEmpty(updatedRequest.Status))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _requestService.UpdateRequest(updatedRequest);

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
