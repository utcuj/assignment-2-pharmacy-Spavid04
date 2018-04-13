using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Filters
{
    public abstract class Filter<T>
    {
        protected List<Predicate<T>> filters = new List<Predicate<T>>();

        public IEnumerable<T> ApplyFilter(IEnumerable<T> source)
        {
            IEnumerable<T> result = source;

            foreach (var filter in filters)
            {
                result = result.Where(x => filter(x) == true);
            }

            return result;
        }

        public void AddFilter(Predicate<T> predicate)
        {
            filters.Add(predicate);
        }

        public bool Matches(T item)
        {
            if (filters.Count == 0) return true;

            return filters
                .Select(x => x(item))
                .All(x => x == true);
        }
    }
}
