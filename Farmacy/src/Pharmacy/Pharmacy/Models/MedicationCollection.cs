using System;
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
