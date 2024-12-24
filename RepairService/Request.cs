using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService
{
    public class Request
    {
        public Guid RequestId { get; set; } // GUID идентификатор
        public DateTime RequestDate { get; set; } // Дата заявки
        public string Equipment { get; set; } // Оборудование
        public string FaultType { get; set; } // Тип неисправности
        public string Description { get; set; } // Описание проблемы
        public string ClientName { get; set; } // Имя клиента
        public string Status { get; set; } // Статус заявки
        public string Responsible { get; set; } // Ответственный
    }



}
