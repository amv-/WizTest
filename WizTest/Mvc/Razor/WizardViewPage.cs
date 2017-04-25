using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WizTest.Models;

namespace WizTest.Mvc.Razor
{
    public abstract class WizardViewPage<TModel, TWizard> : WebViewPage<TModel> where TModel: WizardStep where TWizard: Wizard, new()
    {
        TWizard wizard = null;
        public TWizard Wizard
        {
            get
            {
                return wizard;
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
            wizard = Models.Wizard.GetWizardData<TWizard>();            
        }

        public override void ExecutePageHierarchy()
        {            
            base.ExecutePageHierarchy();
            string output = Output.ToString();
            if (!output.Contains("</body>") && wizard != null)
            {
                AddHidden(Output, "wizstep", wizard.Index.ToString());
                AddHidden(Output, "wiztype", typeof(TWizard).FullName);
            }
        }

        private void AddHidden(TextWriter output, string name, string val)
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("name", name);
            tb.Attributes.Add("id", name);
            tb.Attributes.Add("type", "hidden");
            tb.Attributes.Add("value", val);
            output.Write(tb.ToString());
        }


    }
}