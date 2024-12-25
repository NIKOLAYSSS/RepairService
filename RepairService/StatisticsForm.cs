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
    public partial class StatisticsForm : Form
    {
        private string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=repair_service";

        public StatisticsForm()
        {
            InitializeComponent();
        }

        private void btnGetStatistics_Click(object sender, EventArgs e)
        {
            var statistics = GetStatistics();
            lblCompletedRequests.Text = $"Выполненные заявки: {statistics.CompletedRequests}";
            lblAverageCompletionTime.Text = $"Среднее время выполнения: {statistics.AverageCompletionTime} ч.";
        }

        // Метод для получения статистики
        private (int CompletedRequests, double AverageCompletionTime, DataTable FaultTypeStatistics) GetStatistics()
        {
            int completedRequests = 0;
            double averageCompletionTime = 0;
            DataTable faultTypeStatistics = new DataTable();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Получаем количество выполненных заявок
                string completedRequestsQuery = "SELECT COUNT(*) FROM requests WHERE status = 'выполнено'";
                using (var cmd = new NpgsqlCommand(completedRequestsQuery, connection))
                {
                    completedRequests = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Получаем среднее время выполнения заявки (если нет поля completion_date)
                string averageCompletionTimeQuery = @"
            SELECT AVG(EXTRACT(EPOCH FROM (CURRENT_TIMESTAMP - RequestDate)) / 3600)
            FROM requests
            WHERE status = 'выполнено' AND RequestDate IS NOT NULL";
                using (var cmd = new NpgsqlCommand(averageCompletionTimeQuery, connection))
                {
                    var result = cmd.ExecuteScalar();
                    averageCompletionTime = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                }

                // Получаем статистику по типам неисправностей
                string faultTypeStatisticsQuery = @"
            SELECT FaultType, COUNT(*) AS Count
            FROM requests
            GROUP BY FaultType";
                using (var cmd = new NpgsqlCommand(faultTypeStatisticsQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        faultTypeStatistics.Load(reader);
                    }
                }
            }

            return (completedRequests, averageCompletionTime, faultTypeStatistics);
        }
    }
}
