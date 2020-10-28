using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Utils
{
    public class ESIndexNameAttribute : Attribute
    {
        public string Name { get; }
        public ESIndexNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
