using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pharmacy.Models
{
    [Serializable]
    [XmlRoot("MedicationCollection")]
    public class MedicationCollection
    {
        [XmlArray("Medication")]
        [XmlArrayItem("Medication", typeof(Medication))]
        public Medication[] Medication { get; set; }
    }
}
