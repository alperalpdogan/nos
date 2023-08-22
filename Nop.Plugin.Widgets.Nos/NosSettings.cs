using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.Nos
{
    public class NosSettings : ISettings
    {
        public int LastNumberOfMonths { get; set; }
    }
}
