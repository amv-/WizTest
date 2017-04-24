using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizTest.Models;

namespace WizTest.Controllers
{

    public class WizardController<TWizard, TModel> : Controller 
        where TWizard: WizardBase, new()
        where TModel: WizardStep, new()
    {
        TWizard wizard = WizardBase.GetWizardData<TWizard>();
        WizardDefinition definition = WizardDefinition.GetDefinition<TWizard>();

        public TWizard Wizard
        {
            get
            {
                return wizard;
            }
        }

        public virtual ActionResult Step(TModel model)
        {
            // Clicked Next
            if(model.NextMove == 1)
            {                
                if (ModelState.IsValid)
                {
                    wizard.Steps[wizard.Index] = model;
                    if(wizard.Steps.Count() > wizard.Index + 1)
                    {
                        wizard.Index++;                        
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
                wizard.Index--;
            }

            return PartialView(definition.Steps[wizard.Index].View, wizard.CurrentStep);
        }

        public virtual ActionResult Index()
        {
            return View();
        }
    }
}