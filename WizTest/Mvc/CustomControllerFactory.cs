using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WizTest.Controllers;

namespace WizTest.Mvc
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if(controllerName.EndsWith("wizard", StringComparison.InvariantCultureIgnoreCase))
            {
                string wizTypeStr = requestContext.HttpContext.Request["wiz"];
                string modelTypeStr = requestContext.HttpContext.Request["step"];

                if (!string.IsNullOrWhiteSpace(wizTypeStr))
                {

                    Type modelType = Type.GetType(wizTypeStr);

                    if(modelType != null)
                    {
                        Type genericType = typeof(WizardController<>);
                        Type[] typeArgs = { modelType };
                        Type wizardType = genericType.MakeGenericType(typeArgs);

                        object objController = Activator.CreateInstance(wizardType);

                        if(objController != null)
                        {
                            return (IController)objController;
                        }
                    }

                }
            }
            return base.CreateController(requestContext, controllerName);
        }
    }
}