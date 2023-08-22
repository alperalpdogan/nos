using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.Nos.Models;
using Nop.Plugin.Widgets.Nos.Services;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.Nos.Components
{
    public class WidgetsNosViewComponent : NopViewComponent
    {
        private readonly NosService _nosService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public WidgetsNosViewComponent(NosService nosService,
                                       ISettingService settingService,
                                       IStoreContext storeContext)
        {
            _nosService = nosService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<NosSettings>(storeScope);
            var productId = ((ProductDetailsModel)additionalData).Id;
            var nos = await _nosService.GetNumberOfSalesAsync(productId, settings.LastNumberOfMonths);

            var model = new ProductNosModel()
            {
                NumberOfMonths = settings.LastNumberOfMonths,
                NumberOfSales = nos
            };


            return View("~/Plugins/Widgets.Nos/Views/ProductNos.cshtml", model);
        }
    }
}
