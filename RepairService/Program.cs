using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepairService.DAL;
using RepairService.MODELS;
using RepairService.UI;

namespace RepairService
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string connectionString = "Host=195.46.187.72;Port=5432;Username=postgres;Password=1337;Database=db_repair_service";

            // Создаем экземпляр UserRepository
            IUserService userService = new UserRepository(connectionString);

            // Передаем userService в LoginForm
            Application.Run(new LoginForm(userService));
        }
    }
}
