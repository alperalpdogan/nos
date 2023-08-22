using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Media;
using Nop.Services.Configuration;
using Nop.Core;
using Nop.Plugin.Widgets.Nos.Models;
using Nop.Plugin.Widgets.Nos.Validators;
using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.Nos.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsNosController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;

        public WidgetsNosController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IStaticCacheManager staticCacheManager)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<NosSettings>(storeScope);

            var model = new ConfigurationModel()
            {
                LastNumberOfMonths = settings.LastNumberOfMonths
            };

            return View("~/Plugins/Widgets.Nos/Views/Configure.cshtml", model);

        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var validator = new ConfigurationValidator(_localizationService);
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var warning in validationResult.Errors.Select(error => error.ErrorMessage))
                    ModelState.AddModelError("", warning);

                return await Configure();
            }

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<NosSettings>(storeScope);

            settings.LastNumberOfMonths = model.LastNumberOfMonths;

            //clear nos cache since the setting is changed
            await _staticCacheManager.RemoveByPrefixAsync(NosDefaults.NumberOfSalesForProductPrefix);

            //now clear settings cache
            await _settingService.ClearCacheAsync();
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.LastNumberOfMonths, true, storeScope);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }
    }
}
