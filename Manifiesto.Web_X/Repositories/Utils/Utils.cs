using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manifiesto.Data.Utils
{
    public static class Utils
    {
        public static string iif(bool condition, string t, string f)
        {
            string functionReturnValue = null;
            if (condition)
                functionReturnValue = t;
            else
                functionReturnValue = f;
            return functionReturnValue;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}
