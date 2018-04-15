using System;
using System.Collections.Generic;

namespace Pharmacy.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string ClientIdentifier { get; set; }
        public DateTime Date { get; set; }

        public virtual User Issuer { get; set; }
        public virtual List<InvoiceContents> Medications { get; set; }
    }
}
