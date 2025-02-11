using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.MODELS
{
    public interface IRequestService
    {
        void AddRequest(Request request);
        List<Request> GetRequests();
        void UpdateRequest(Request request);
        bool DeleteRequest(Guid requestId);
        Request GetRequestById(Guid requestId);
        List<Request> SearchRequests(string searchTerm);
    }
}
