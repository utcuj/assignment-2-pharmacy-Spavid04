using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Database
{
    public static class ConnectionFactory
    {
        public static DbContext GetDbContext(string applicationName)
        {
            switch (applicationName)
            {
                case "Pharmacy":
                    return new PharmacyDbContext();
            }

            return null;
        }
    }
}
