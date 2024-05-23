using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.UserDto
{
    public class UserNewPassword
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordVerif { get; set; }

        public string NewPassword { get; set; }
    }
}
