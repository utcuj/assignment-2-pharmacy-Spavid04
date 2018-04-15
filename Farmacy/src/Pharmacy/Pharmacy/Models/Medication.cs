using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pharmacy.Models
{
    [Serializable]
    public class Medication
    {
        [XmlElement(nameof(Id))]
        public int Id { get; set; }

        [XmlElement(nameof(Name))]
        public string Name { get; set; }
        [XmlElement(nameof(Manufacturer))]
        public string Manufacturer { get; set; }
        [XmlElement(nameof(Ingredients))]
        public string Ingredients { get; set; } //comma separated

        [XmlElement(nameof(Price))]
        public int Price { get; set; }
        [XmlElement(nameof(Stock))]
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"{Name}  -  {Manufacturer}  -  ¤{Price}";
        }

        public string ToCsvRow()
        {
            string[] cells = new string[]
            {
                Id.ToString(),
                Name,
                Manufacturer,
                Ingredients,
                Price.ToString(),
                Stock.ToString()
            };

            return String.Join(",", cells.Select(x => x.EscapeCsvCell()));
        }
    }
}
