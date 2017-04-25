using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WizTest.Controllers;
using WizTest.Models;

namespace WizTest.Mvc
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            string wizTypeStr = requestContext.HttpContext.Request["wiztype"];
            string wizStep = requestContext.HttpContext.Request["wizstep"];


            IController controller = null;
            if (!string.IsNullOrWhiteSpace(wizTypeStr) && !string.IsNullOrWhiteSpace(wizStep))
            {
                int stepIndex = -1;
                if (int.TryParse(wizStep, out stepIndex) && stepIndex >= 0)
                {
                    Type type = Type.GetType(wizTypeStr);
                    if (type != null)
                    {
                        controller = GetWizardController(type, stepIndex);
                    }
                }
            }

            return controller ?? base.CreateController(requestContext, controllerName);
        }

        protected virtual IController GetWizardController(Type wizardType, int stepIndex)
        {
            var definition = WizardDefinition.GetDefinition(wizardType);
            if(definition != null && stepIndex >= 0 && definition.Steps.Count > stepIndex)
            {
                return definition.Steps[stepIndex].CreateController();
            }

            //if (controllerName.Equals("wizard", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    Type genericType = typeof(WizardController<,>);
            //    var definition = WizardDefinition.GetDefinition(wizardType);

            //    if (definition != null && stepIndex > -1 && stepIndex < definition.Steps.Count)
            //    {
            //        var modelType = definition.Steps[stepIndex].Type;

            //        if (modelType != null)
            //        {
            //            Type[] typeArgs = { wizardType, modelType };

            //            Type controllerType = genericType.MakeGenericType(typeArgs);

            //            object objController = Activator.CreateInstance(wizardType);

            //            if (objController != null)
            //            {
            //                return (IController)objController;
            //            }
            //        }
            //    }
            //}
            return null;
        }
    }
}