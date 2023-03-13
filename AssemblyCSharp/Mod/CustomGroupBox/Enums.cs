using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.CustomGroupBox
{
    public enum StateGroupBox
    {
        Showed = 0x1,
        Collapsed = 0x2,
        Hided = 0x4,
        Collapsing = 0x20000000,
        Expanding = 0x40000000
    }
}
