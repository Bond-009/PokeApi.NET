using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.NET.Tests
{
    public static class Extensions
    {

        public static bool AnyStringPropertyNullOrEmpty(this object obj)
        {
            return obj.GetType().GetProperties()
                    .Where(p => p.GetValue(obj) is string) // selecting only string props
                    .Any(p => string.IsNullOrWhiteSpace((p.GetValue(obj) as string)));
        }
    }
}
