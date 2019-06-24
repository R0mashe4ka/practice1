using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practice1.Models
{
    public class UserDTO
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        //public Company Company { get; set; }

    }
}
