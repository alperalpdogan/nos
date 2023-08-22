using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.Nos
{
    public class NosDefaults
    {
        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Widgets.Nos.Configure";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string NumberOfSalesForProductPrefix => "Nop.nos.numberofsalesforproduct";

        /// <summary>
        /// Gets the key for number of sales
        /// </summary>
        /// <remarks>
        /// {0} : Product Id
        /// </remarks>
        public static CacheKey NumberOfSalesForProductCacheKey => new("Nop.nos.numberofsalesforproduct.{0}", NumberOfSalesForProductPrefix);

        /// <summary>
        /// Number of months to show the sales from 
        /// defaults to 1
        /// </summary>
        public static int LastNumberOfMonthsForSales => 1;

    }
}
