using Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();

        void AddUser(CreateUser user);

        void UpdateUser(CreateUser user, Guid Id);

        void DeleteUser(Guid Id);
    }
}
