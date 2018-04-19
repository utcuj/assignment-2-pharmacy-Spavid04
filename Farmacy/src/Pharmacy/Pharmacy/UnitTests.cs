using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Pharmacy.Controllers;
using Pharmacy.Database;
using Pharmacy.Models;

namespace Pharmacy
{
    [TestFixture]
    class UnitTests
    {
        private PharmacyDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            dbContext = new PharmacyDbContext();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.InvoiceContents.RemoveRange(dbContext.InvoiceContents);
            dbContext.Invoices.RemoveRange(dbContext.Invoices);
            dbContext.Users.RemoveRange(dbContext.Users);
            dbContext.Medications.RemoveRange(dbContext.Medications);
            dbContext.SaveChanges();

            dbContext.Dispose();
        }

        [Test]
        public void TestAddUserAndLogin()
        {
            var user = AdministratorController.AddOrUpdateUser(0, "user", "name", "password", UserType.Chemist);

            Assert.True(user.Id != 0);

            Assert.True(LoginController.TryLogin("user", "password"));
            Assert.False(LoginController.TryLogin("user", "pwd"));
            Assert.False(LoginController.TryLogin("something", "something"));

            Assert.DoesNotThrow(()=>LoginController.GetUserLevelAndId("user"));
            Assert.Throws<InvalidOperationException>(() => LoginController.GetUserLevelAndId("something"));
        }

        [Test]
        public void TestAddMedicationAndSaveOrder()
        {
            var user = AdministratorController.AddOrUpdateUser(0, "user", "name", "password", UserType.Chemist);

            var medication =
                AdministratorController.AddOrUpdateMedication(0, "medication", "manufacturer", "a,b,c", 10, 10);

            Assert.True(medication.Id != 0);

            medication = AdministratorController.GetMedication(medication.Id);

            Dictionary<int, int> contents = new Dictionary<int, int>()
            {
                {medication.Id, 9}
            };
            
            var invoice = RegularUserController.SaveOrder(user.Id, "client", contents);
            medication = RegularUserController.GetMedication(medication.Id);

            Assert.True(invoice.Id != 0);
            Assert.True(invoice.Issuer.Id == user.Id);
            Assert.True(medication.Stock == 1);
            Assert.True(invoice.Medications.Count == 1);
        }

        [Test]
        public void TestDeleteMedication()
        {
            var medication =
                AdministratorController.AddOrUpdateMedication(0, "medication", "manufacturer", "a,b,c", 10, 10);

            AdministratorController.DeleteMedication(medication.Id);

            Assert.Throws<InvalidOperationException>(()=>AdministratorController.GetMedication(medication.Id));
        }

        [Test]
        public void TestDeleteUser()
        {
            var user = AdministratorController.AddOrUpdateUser(0, "user", "name", "password", UserType.Administrator);

            AdministratorController.DeleteUser(user.Id);

            Assert.Throws<InvalidOperationException>(() => AdministratorController.GetUser(user.Id));
        }
    }
}
