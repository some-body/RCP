using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools
{
    public static class ShuffleListExtention
    {
        public static IEnumerable<T> RandomSelect<T>(this IEnumerable<T> list, int count)
        {
            var rnd = new Random(DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
            return list.OrderBy(x => rnd.Next()).Take(count);
        }
    }
}
