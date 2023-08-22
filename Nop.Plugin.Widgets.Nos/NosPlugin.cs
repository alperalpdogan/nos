using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.FileProviders;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.Nos.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.Nos
{
    public class NosPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public NosPlugin(ILocalizationService localizationService,
                         ISettingService settingService,
                         IUrlHelperFactory urlHelperFactory,
                         IActionContextAccessor actionContextAccessor)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(NosDefaults.ConfigurationRouteName);

        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsAfterPictures });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsNosViewComponent);
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            var settings = new NosSettings()
            {
                LastNumberOfMonths = NosDefaults.LastNumberOfMonthsForSales
            };

            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.Nos.LastNumberOfMonths"] = "Sales to be shown in the last number of months",
                ["Plugins.Widgets.Nos.MonthsOutOfRange"] = "Number of months should be in between 1-12",
                ["Plugins.Widgets.Nos.SoldInLastMonth"] = "Sold {0} in the last month",
                ["Plugins.Widgets.Nos.SalesInNumberOfMonths"] = "Sold {0} in the last {1} months"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<NosSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.Nos");

            await base.UninstallAsync();
        }

        public bool HideInWidgetList => false;
    }
}