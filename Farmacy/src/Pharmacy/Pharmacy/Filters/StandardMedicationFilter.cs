using System;
using System.Linq;
using Pharmacy.Models;

namespace Pharmacy.Filters
{
    class StandardMedicationFilter : Filter<Medication>
    {
        public StandardMedicationFilter(string byName, string byManufacturer, string byIngredients)
        {
            if (!String.IsNullOrEmpty(byName))
            {
                this.AddFilter(x => x.Name.ToUpper().Contains(byName));
            }
            if (!String.IsNullOrEmpty(byManufacturer))
            {
                this.AddFilter(x => x.Manufacturer.ToUpper().Contains(byManufacturer));
            }
            if (!String.IsNullOrEmpty(byIngredients))
            {
                string[] ingredients = byIngredients.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                this.AddFilter(x =>
                {
                    string[] mIngredients = x.Ingredients.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return mIngredients.Any(y => ingredients.Contains(y.NormalizeString()));
                });
            }
        }
    }
}
