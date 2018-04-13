using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class Medication
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Ingredients { get; set; } //comma separated

        public int Price { get; set; }
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Manufacturer}";
        }
    }
}
