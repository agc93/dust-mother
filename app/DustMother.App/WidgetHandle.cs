using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DustMother.App
{
    public class WidgetHandle
    {
        public CoreWindow Window { get; set; }
        public XboxGameBarWidget Widget { get; set; }
    }
}
