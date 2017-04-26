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
            string wizName = requestContext.HttpContext.Request["wizname"];
            string wizStep = requestContext.HttpContext.Request["wizstep"];


            if (!string.IsNullOrWhiteSpace(wizName) && !string.IsNullOrWhiteSpace(wizStep))
            {
                int stepIndex = -1;
                if (int.TryParse(wizStep, out stepIndex) && stepIndex >= 0)
                {
                    var definition = WizardDefinition.GetDefinition(wizName);
                    if(definition != null && stepIndex < definition.Steps.Count)
                    {
                        return definition.Steps[stepIndex].CreateController();
                    }
                }
            }

            return base.CreateController(requestContext, controllerName);
        }

    }
}