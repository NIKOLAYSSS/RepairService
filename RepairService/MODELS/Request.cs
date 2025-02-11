using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.MODELS
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public DateTime RequestDate { get; set; }

        // Добавляем свойства для справочных данных:
        public string Equipment { get; set; }      // Название оборудования
        public string FaultType { get; set; }      // Название типа неисправности
        public string Description { get; set; }
        public string ClientName { get; set; }     // Имя клиента
        public string Status { get; set; }         // Название статуса заявки
        public string Responsible { get; set; }    // Имя ответственного (может быть null)
    }



}
