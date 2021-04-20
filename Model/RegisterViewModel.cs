using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPosgre.Model
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
