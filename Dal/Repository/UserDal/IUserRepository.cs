﻿using Microsoft.Data.SqlClient;
using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.User;
using Models.Entity.UserModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository.UserDal
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User? GetById(Guid id);

        User? AddUser(CreateUser user);

        User? UpdateUser(UpdateUser user, Guid id);

        bool DeleteUser(Guid id);

        bool DeleteUserFromDatabase(Guid id);

        bool Exist(User user);

        User? Login(UserLogin user);

        User? UpdatePassword(UserNewPassword user, Guid id);
    }
}
