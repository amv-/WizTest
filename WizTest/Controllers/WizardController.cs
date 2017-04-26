using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WizTest.Models;

namespace WizTest.Controllers
{

    public class WizardController<TModel> : Controller
        where TModel: WizardStep, new()
    {
        public WizardDefinition Definition { set; get; }
        public Wizard Wizard { set; get; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string wizName = HttpContext.Request["wizname"];
            if (!string.IsNullOrWhiteSpace(wizName))
            {
                Definition = WizardDefinition.GetDefinition(wizName);
                Wizard = Wizard.GetWizardData(wizName);
                HttpContext.Items[WizardDefinition.ContextName] = Wizard;
            }

        }


        public virtual ActionResult Step(TModel model)
        {
            // Clicked Next
            if(model.NextMove == 1)
            {                
                if (ModelState.IsValid)
                {
                    Wizard.Steps[Wizard.Index] = model;

                    // final step
                    if(Wizard.Index == Wizard.Steps.Length - 1)
                    {
                        return FinalStep(model);
                    }
                    if(Wizard.Steps.Count() > Wizard.Index + 1)
                    {
                        Wizard.Index++;                        
                    }
                }
                else
                {
                    model.SetStatus(MessageType.Danger, "Something bad happened");
                }
            }

            // Clicked prev
            if(model.NextMove == -1)
            {
                Wizard.Index--;
            }

            return PartialView(Definition.Steps[Wizard.Index].View, Wizard.CurrentStep);
        }

        public virtual ActionResult FinalStep(TModel model)
        {
            return PartialView(Definition.Steps[Wizard.Index].View, Wizard.CurrentStep);
        }

        public virtual ActionResult Index()
        {
            return View();
        }
    }


}