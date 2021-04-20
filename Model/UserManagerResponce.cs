using System;
using System.Collections.Generic;

namespace IdentityPosgre.Model
{
    public class UserManagerResponce
    {
        public string Message { get; set; }
        public bool IsSuccsess { get; set; }
        public List<string> Errors { get; set; }
        public DateTime ExireDate { get; set; }
    }
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
