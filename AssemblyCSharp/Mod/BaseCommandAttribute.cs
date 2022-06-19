using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class BaseCommandAttribute: Attribute
    {
        public char delimiter = ' ';
    }
}
