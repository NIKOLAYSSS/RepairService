using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.MODELS
{
    public interface IUserService
    {
        void RegisterUser(string username, string passwordHash);
        string GetPasswordHash(string username);
        string GetUserRole(string username);
    }
}
