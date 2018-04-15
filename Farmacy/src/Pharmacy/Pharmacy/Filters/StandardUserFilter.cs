using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Models;

namespace Pharmacy.Filters
{
    public class StandardUserFilter : Filter<User>
    {
        public StandardUserFilter(string byUsername, string byName, UserType? byType)
        {
            if (!String.IsNullOrEmpty(byUsername))
            {
                this.AddFilter(x => x.Username.ToUpper().Contains(byUsername));
            }
            if (!String.IsNullOrEmpty(byName))
            {
                this.AddFilter(x => x.Name.ToUpper().Contains(byName));
            }
            if (byType != null)
            {
                this.AddFilter(x => x.UserType == byType);
            }
        }
    }
}
