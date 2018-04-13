using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Models;

namespace Pharmacy.Database
{
    class PharmacyDbContext : DbContext
    {
        public const string ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=pharmacy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public PharmacyDbContext()
        {
            this.Database.Connection.ConnectionString = ConnectionString;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceContents> InvoiceContents { get; set; }
    }
}
