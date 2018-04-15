namespace Pharmacy.Models
{
    public class InvoiceContents
    {
        public int Id { get; set; }

        public int Amount { get; set; }
        public virtual Medication Medication { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
