using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResponsePropertyNameAttribute : Attribute
    {
        public ResponsePropertyNameAttribute(string name)
        {
            this.Name = name;
        }
        public ResponsePropertyNameAttribute(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
