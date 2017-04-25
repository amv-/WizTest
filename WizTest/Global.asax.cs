using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WizTest.Controllers;
using WizTest.Models;
using WizTest.Mvc;

namespace WizTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory());

            WizardDefinition defMyWizard = new WizardDefinition();
            defMyWizard
                .Add<Step1, MyWizard, WizardController<MyWizard, Step1>>("~/Views/Wizard/MyWizard/Step1.cshtml")
                .Add<Step2, MyWizard, WizardController<MyWizard, Step2>>("~/Views/Wizard/MyWizard/Step2.cshtml");

            WizardDefinition.Register<MyWizard>(defMyWizard);
        }
    }
}
