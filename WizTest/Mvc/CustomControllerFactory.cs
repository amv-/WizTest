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
            if(controllerName.EndsWith("wizard", StringComparison.InvariantCultureIgnoreCase))
            {
                string wizTypeStr = requestContext.HttpContext.Request["wiztype"];
                string wizStep = requestContext.HttpContext.Request["wizstep"];
                int currentStep = -1;
                ;
                if (!string.IsNullOrWhiteSpace(wizTypeStr) && int.TryParse(wizStep, out currentStep) & currentStep > -1)
                {

                    Type wizType = Type.GetType(wizTypeStr);

                    if(wizType != null)
                    {
                        Type genericType = typeof(WizardController<,>);
                        var definition = WizardDefinition.GetDefinition(wizType);

                        if(definition != null && currentStep > -1)
                        {
                            var modelType = definition.Steps[currentStep].Type;
                            if(modelType != null)
                            {
                                Type[] typeArgs = { wizType, modelType };
                                Type wizardType = genericType.MakeGenericType(typeArgs);

                                object objController = Activator.CreateInstance(wizardType);

                                if (objController != null)
                                {
                                    return (IController)objController;
                                }
                            }
                        }

                    }

                }
            }
            return base.CreateController(requestContext, controllerName);
        }
    }
}