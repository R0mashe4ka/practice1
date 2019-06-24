using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace practice1.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public ICollection <User> Users { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyLocation { get; set; }

    }
}
