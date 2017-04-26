using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            WizardDefinition defMyWizard = new WizardDefinition("MyWizard");
            defMyWizard
                .Add<Step1>("~/Views/Wizard/MyWizard/Step1.cshtml")
                .Add<Step2>("~/Views/Wizard/MyWizard/Step2.cshtml");

            WizardDefinition.Register(defMyWizard);

            InitSimpleInjector();
        }

        public void InitSimpleInjector()
        {
            // Wire up simpleinjector
            var container = new Container();
            container.Options.ConstructorResolutionBehavior = new GreediestConstructorBehavior();
            container.Options.DefaultLifestyle = Lifestyle.Transient;
            container.Options.AllowOverridingRegistrations = true;

            container.Register<IDatabase>(() => new SomeDb());

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }

    public class GreediestConstructorBehavior : IConstructorResolutionBehavior
    {
        public ConstructorInfo GetConstructor(Type implementationType) => (
            from ctor in implementationType.GetConstructors()
            orderby ctor.GetParameters().Length descending
            select ctor)
            .FirstOrDefault();
    }
}
