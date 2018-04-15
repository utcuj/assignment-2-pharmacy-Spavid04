using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public enum UserType
    {
        Chemist = 0,
        Administrator = 1
    }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public UserType UserType { get; set; }

        public virtual List<Invoice> IssuedInvoices { get; set; }

        public override string ToString()
        {
            return $"{Name}  -  {Username}";
        }
    }
}
