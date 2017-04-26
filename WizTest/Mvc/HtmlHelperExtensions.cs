using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizTest.Models;
using System.Web.Mvc.Html;

namespace WizTest.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Wizard(this HtmlHelper helper, string name)
        {
            var definition = WizardDefinition.GetDefinition(name);
            if(definition == null)
            {
                return new MvcHtmlString("No wizard with name " + name + " was registered");
            }
            var wizard = WizTest.Models.Wizard.GetWizardData(name);
            HttpContext.Current.Items[WizardDefinition.ContextName] = wizard;
            return helper.Partial("WizardContainer", wizard);
        }
    }
}