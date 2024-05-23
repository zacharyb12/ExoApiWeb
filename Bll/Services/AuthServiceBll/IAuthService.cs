using Models.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services.AuthServiceBll
{
    public interface IAuthService
    {
        string GenerateToken(User user);

    }
}
