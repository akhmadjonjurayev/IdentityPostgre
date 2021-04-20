using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPosgre.Model
{
    public class UserDataResponse<T>
    {
        public bool IsSuccsess { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
