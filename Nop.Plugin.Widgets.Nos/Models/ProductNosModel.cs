using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Nos.Models
{
    public record ProductNosModel : BaseNopModel
    {
        public int NumberOfMonths { get; set; }

        public int NumberOfSales { get; set; }          
    }
}
