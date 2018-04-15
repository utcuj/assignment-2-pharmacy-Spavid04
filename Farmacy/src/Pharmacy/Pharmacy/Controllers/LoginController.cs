using System;
using System.Linq;
using Pharmacy.Database;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    public static class LoginController
    {
        public static bool TryLogin(string username, string password)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            var user = dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            return (user != null);
        }

        public static Tuple<int, UserType> GetUserLevelAndId(string username)
        {
            var dbContext = ConnectionFactory.GetDbContext("Pharmacy") as PharmacyDbContext;

            var user = dbContext.Users.First(x => x.Username == username);

            return new Tuple<int, UserType>(user.Id, user.UserType);
        }
    }
}
