using Bll.CustomsExeptions;
using Bll.Services.AuthServiceBll;
using Dal.Repository.UserDal;
using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.User;
using Models.Entity.UserModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services.UserServiceBll
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository,IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public User AddUser(CreateUser user)
        {
            return _userRepository.AddUser(user);
        }

        public bool DeleteUser(Guid id)
        {
            return _userRepository.DeleteUser(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User? UpdateUser(UpdateUser user, Guid id)
        {
            User? u = _userRepository.GetById(id);
            if(_userRepository.Exist(u))
            {
                throw new AlreadyExistException("Email already exist");
            }
            if (u != null) 
            {
                return _userRepository.UpdateUser(user, id);
            }
            return null;
        }

        public User? UpdatePassword(UserNewPassword user, Guid id)
        {
            if (user.Password == user.PasswordVerif)
            {
                return _userRepository.UpdatePassword(user, id);
            }
            return null;
        }

        public string? Login(UserLogin user)
        {
            User? userToken = _userRepository.Login(user);

            if (userToken is not null)
            {
                string? token = _authService.GenerateToken(userToken);

                return token;
            }

            return null;
        }

        public bool DeleteUserFromDatabase(Guid id)
        {
            return _userRepository.DeleteUserFromDatabase(id);
        }

        public bool Exist(User user)
        {
            return _userRepository.Exist(user);
        }
    }
}
