using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteKarloMatrixVectorProduct
{
    public static class Extensions
    {
        public static T Pop<T>(this IList<T> source)
        {
            var item = source.Last();
            source.Remove(item);

            return item;
        }
    }
}


