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
    public abstract class WizardViewPage<TModel> : WebViewPage<TModel> where TModel: WizardStep
    {
        public Wizard Wizard { get; protected set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Wizard = HttpContext.Current.Items[WizardDefinition.ContextName] as Wizard;
        }

        public override void ExecutePageHierarchy()
        {            
            base.ExecutePageHierarchy();
            string output = Output.ToString();
            if (!output.Contains("</body>") && Wizard != null)
            {
                AddHidden(Output, "wizstep", Wizard.Index.ToString());
                AddHidden(Output, "wizname", Wizard.Definition.Name);
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