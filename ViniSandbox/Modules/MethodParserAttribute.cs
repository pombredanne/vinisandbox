using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViniSandbox.Modules
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MethodParserAttribute : Attribute
    {
        public MethodParserAttribute()
        {
        }
    }
}
