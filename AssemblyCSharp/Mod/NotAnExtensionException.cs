using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    public class NotAnExtensionException : Exception
    {
        public NotAnExtensionException() : base() { }

        public NotAnExtensionException(string message) : base(message) { }
    }
}
