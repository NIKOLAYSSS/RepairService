using RepairService.MODELS;
using RepairService.DAL;
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
    public partial class AddRequestForm : Form
    {
        private readonly IRequestService _requestService;

        public AddRequestForm(IRequestService requestService)
        {
            InitializeComponent();
            _requestService = requestService;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var request = new Request
                {
                    RequestId = Guid.NewGuid(),
                    Equipment = txtEquipment.Text.Trim(),
                    FaultType = txtFaultType.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    ClientName = txtClientName.Text.Trim(),
                    Responsible = txtResponsible.Text.Trim(),
                    Status = cmbStatus.SelectedItem?.ToString() ?? "в ожидании",
                    RequestDate = dtpRequestDate.Value
                };

                if (string.IsNullOrEmpty(request.Equipment) ||
                    string.IsNullOrEmpty(request.FaultType) ||
                    string.IsNullOrEmpty(request.ClientName))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _requestService.AddRequest(request);

                MessageBox.Show("Заявка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
