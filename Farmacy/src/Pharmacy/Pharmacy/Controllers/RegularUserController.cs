using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Database;
using Pharmacy.Filters;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    public static class RegularUserController
    {
        public static IEnumerable<Medication> SearchMedication(string name, string manufacturer, string ingredients)
        {
            Filter<Medication> filter = new MedicationFilter(name.NormalizeString(), manufacturer.NormalizeString(), ingredients.NormalizeString());

            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return filter.ApplyFilter(dbContext.Medications);
        }

        public static Medication GetMedication(int id)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return dbContext.Medications.First(x => x.Id == id);
        }
    }
}
