using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizTest.Models;

namespace WizTest.Controllers
{
    public class WizardController<TWizard, TStep> : Controller where TWizard : WizardBase where TStep : WizardStepModel
    {
        // GET: Wizard
        public ActionResult Index()
        {
            TWizard wizard = WizardBase.GetWizardData<TWizard>();
            
            return Content("index, uuu");
        }

        public ActionResult Step(TStep model)
        {
            return Content("Wuuu, " + typeof(TStep).ToString());
        }
    }

    public class Wizard
    {

    }
}