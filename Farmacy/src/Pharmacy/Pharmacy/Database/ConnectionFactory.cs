using System.Data.Entity;

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
