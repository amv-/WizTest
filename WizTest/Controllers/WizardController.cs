using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizTest.Models;

namespace WizTest.Controllers
{
    public class WizardController<T> : Controller where T : WizardStepModel
    {
        // GET: Wizard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Step(int index)
        {

        }
    }

    public class Wizard
    {

    }
}