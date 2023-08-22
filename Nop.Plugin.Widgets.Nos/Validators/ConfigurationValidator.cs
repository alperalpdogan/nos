using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Widgets.Nos.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Nos.Validators
{
    public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
    {
        public ConfigurationValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.LastNumberOfMonths)
                .InclusiveBetween(1, 12)
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.Nos.MonthsOutOfRange"));
        }
    }
}
