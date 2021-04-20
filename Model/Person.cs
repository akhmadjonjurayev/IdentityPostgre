using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPosgre.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Region { get; set; }
        public DateTime Birthday { get; set; }
        public string PasswordSeria { get; set; }
        public string Malumoti { get; set; }
        public string PhoneNumber { get; set; }
        public string CardID { get; set; }
        public string INN { get; set; }
    }
}
