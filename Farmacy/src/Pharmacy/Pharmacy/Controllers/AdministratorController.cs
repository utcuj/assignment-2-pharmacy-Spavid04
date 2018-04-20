using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Pharmacy.Database;
using Pharmacy.Filters;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    public static class AdministratorController
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

        public static void DeleteMedication(int id)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            dbContext.Medications.Remove(dbContext.Medications.First(x => x.Id == id));
            dbContext.SaveChanges();
        }

        public static Medication AddOrUpdateMedication(int id, string name, string manufacturer, string ingredients, int price, int stock)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            var medication = new Medication();

            medication.Id = id;
            medication.Name = name;
            medication.Manufacturer = manufacturer;
            medication.Ingredients = ingredients;
            medication.Price = price;
            medication.Stock = stock;

            dbContext.Medications.AddOrUpdate(medication);
            dbContext.SaveChanges();

            return medication;
        }

        public static IEnumerable<User> SearchChemists(string username, string name)
        {
            Filter<User> filter =
                new StandardUserFilter(username.NormalizeString(), name.NormalizeString(), UserType.Chemist);

            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return filter.ApplyFilter(dbContext.Users);
        }

        public static User GetUser(int id)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            return dbContext.Users.First(x => x.Id == id);
        }

        public static User AddOrUpdateUser(int id, string username, string name, string password, UserType type)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            var user = new User();

            user.Id = id;
            user.Username = username;
            user.Name = name;
            user.Password = password;
            user.UserType = type;

            dbContext.Users.AddOrUpdate(user);
            dbContext.SaveChanges();

            return user;
        }

        public static void DeleteUser(int id)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            dbContext.Users.Remove(dbContext.Users.First(x => x.Id == id));
            dbContext.SaveChanges();
        }

        public static void GenerateReport(string type)
        {
            if (type == "CSV")
            {
                using (FileStream fs = new FileStream("report.csv", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

                        sw.WriteLine(String.Join(",", "Id", "Name", "Manufacturer", "Ingredients", "Price",
                            "Last sold date"));

                        foreach (var medication in dbContext.Medications.Where(x => x.Stock == 0))
                        {
                            sw.WriteLine(MedicationToCsvRow(medication));
                        }
                    }
                }
            }
            else if (type == "PDF")
            {
                using (FileStream fs = new FileStream("report.pdf", FileMode.Create))
                {
                    PdfDocument doc = new PdfDocument(fs);
                    
                    XFont font = new XFont("Verdana", 20);

                    var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;
                    StringBuilder sb = new StringBuilder(String.Join(" - ", "Id", "Name", "Manufacturer", "Ingredients",
                                                             "Price", "Last sold date") + Environment.NewLine);

                    foreach (var medication in dbContext.Medications.Where(x => x.Stock == 0))
                    {
                        sb.AppendLine(MedicationToCsvRow(medication, " - "));
                    }

                    PdfPage page = doc.AddPage();
                    XGraphics graph = XGraphics.FromPdfPage(page);
                    XTextFormatter formatter = new XTextFormatter(graph);
                    formatter.DrawString(sb.ToString(), font, XBrushes.Black,
                        new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                    doc.Close();
                }
            }
        }

        private static string MedicationToCsvRow(Medication m, string separator = ",")
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            var lastInvoice = dbContext.Invoices.Where(x => x.Medications.Any(y => y.Medication.Id == m.Id))
                .OrderByDescending(x => x.Date).FirstOrDefault();

            string[] cells = new string[]
            {
                m.Id.ToString(),
                m.Name,
                m.Manufacturer,
                m.Ingredients,
                m.Price.ToString(),
                lastInvoice?.Date.ToLocalTime().ToString() ?? "never sold"
            };

            return String.Join(separator, cells.Select(x => x.EscapeCsvCell()));
        }

        public static void ExportMedication()
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            MedicationCollection mc = new MedicationCollection();
            mc.Medication = dbContext.Medications.ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(MedicationCollection));
            using (FileStream fs = new FileStream("medication.xml", FileMode.Create))
            {
                serializer.Serialize(fs, mc);
            }
        }

        public static void ImportMedication(string file)
        {
            MedicationCollection mc;

            XmlSerializer serializer = new XmlSerializer(typeof(MedicationCollection));
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                mc = serializer.Deserialize(fs) as MedicationCollection;
            }

            if (mc != null)
            {
                var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

                foreach (var medication in mc.Medication)
                {
                    dbContext.Medications.AddOrUpdate(medication);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
