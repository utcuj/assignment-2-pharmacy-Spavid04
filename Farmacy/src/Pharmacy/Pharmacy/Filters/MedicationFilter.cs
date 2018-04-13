using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Models;

namespace Pharmacy.Filters
{
    class MedicationFilter : Filter<Medication>
    {
        public MedicationFilter(string byName, string byManufacturer, string byIngredients)
        {
            if (!String.IsNullOrEmpty(byName))
            {
                this.AddFilter(x => x.Name.ToUpper() == byName);
            }
            if (!String.IsNullOrEmpty(byManufacturer))
            {
                this.AddFilter(x => x.Manufacturer.ToUpper() == byManufacturer);
            }
            if (!String.IsNullOrEmpty(byIngredients))
            {
                string[] ingredients = byIngredients.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                this.AddFilter(x =>
                {
                    string[] mIngredients = x.Ingredients.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return ingredients.Any(y => mIngredients.Contains(y));
                });
            }
        }
    }
}
