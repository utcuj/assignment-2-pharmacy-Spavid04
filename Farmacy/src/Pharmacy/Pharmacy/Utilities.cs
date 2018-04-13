using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy
{
    public static class Utilities
    {
        public static string NormalizeString(this string input)
        {
            return input.Replace(" ", "").ToUpper();
        }
    }
}
