using Dal.Repository.UserDal;
using Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(CreateUser user)
        {
            _userRepository.AddUser(user);
        }

        public void DeleteUser(Guid Id)
        {
            _userRepository.DeleteUser(Id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public void UpdateUser(CreateUser user , Guid Id)
        {
           _userRepository.UpdateUser(user,Id);
        }
    }
}
