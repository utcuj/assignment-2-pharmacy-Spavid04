using System;
using System.Collections.Generic;
using System.Linq;
using Pharmacy.Database;
using Pharmacy.Filters;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    public static class RegularUserController
    {
        public static IEnumerable<Medication> SearchMedication(string name, string manufacturer, string ingredients)
        {
            Filter<Medication> filter = new StandardMedicationFilter(name.NormalizeString(), manufacturer.NormalizeString(), ingredients.NormalizeString());

            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return filter.ApplyFilter(dbContext.Medications);
        }

        public static Medication GetMedication(int id)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return dbContext.Medications.First(x => x.Id == id);
        }

        public static Invoice SaveOrder(int userId, string client, Dictionary<int, int> contents)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            Invoice invoice = new Invoice();
            
            invoice.ClientIdentifier = client;
            invoice.Date = DateTime.UtcNow;
            invoice.Issuer = dbContext.Users.First(x => x.Id == userId);

            dbContext.Invoices.Add(invoice);

            foreach (var item in contents)
            {
                InvoiceContents ic = new InvoiceContents();

                ic.Amount = item.Value;
                ic.Invoice = invoice;
                ic.Medication = dbContext.Medications.First(x => x.Id == item.Key);

                dbContext.Medications.First(x => x.Id == item.Key).Stock -= item.Value;

                dbContext.InvoiceContents.Add(ic);
            }

            dbContext.SaveChanges();

            return invoice;
        }
    }
}
