using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizTest.Controllers;

namespace WizTest.Models
{
    public class AppViewModel
    {
        public StatusMessage StatusMessage { set; get; }

        public void SetStatus(MessageType messageType, string message)
        {
            StatusMessage = new StatusMessage { MessageType = messageType, Message = message };
        }
    }

    public class WizardStep : AppViewModel
    {
        public int NextMove { set; get; }
    }


    #region Steps

    public class Step1 : WizardStep
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

    }

    public class Step2 : WizardStep
    {

        public string Brand { set; get; }
        public string Logo { set; get; }
    }

    #endregion

    public class Wizard
    {
        public WizardDefinition Definition { get; private set; }

        public int Index { set; get; } = 0;

        public WizardStep[] Steps { get; set; }

        public Wizard(WizardDefinition definition)
        {

            if (definition == null)
            {
                throw new InvalidOperationException("Wizard type definition not provided");
            }
            Definition = definition;
            Steps = new WizardStep[definition.Steps.Count];

        }

        public WizardStep CurrentStep
        {
            get
            {
                return Steps[Index];
            }
        }

        #region Static stuff
        const string WIZ_PREFIX = "Wizard_";
        public static Wizard GetWizardData(string name)
        {
            var wizard = HttpContext.Current.Session[WIZ_PREFIX + name] as Wizard;
            return wizard ?? Reset(name);
        }

        public static Wizard Reset(string name)
        {
            var def = WizardDefinition.GetDefinition(name);
            var wizard = new Wizard(def);
            HttpContext.Current.Session[WIZ_PREFIX + name] = wizard;
            return wizard;
        }
        #endregion

    }


    public interface IWizardStepMeta
    {
        string View { get; }
        WizardStep CreateStepModel();
        Type Type { get; }
        WizardDefinition WizardDefinition { get; }
        IController CreateController();
    }

    public class WizardStepMeta<TModel> : IWizardStepMeta where TModel : WizardStep, new()
    {
        public Func<TModel, ActionResult> action;

        public WizardDefinition WizardDefinition { get; protected set; }
        public WizardStepMeta(string view, WizardDefinition definition, Func<TModel, ActionResult> action = null)
        {
            View = view;
            this.action = action;
            WizardDefinition = definition;
        }

        public string View { private set; get; }

        WizardStep IWizardStepMeta.CreateStepModel()
        {
            return new TModel();
        }

        public IController CreateController()
        {     
            return new WizardController<TModel>();
        }

        Type _type;
        public Type Type
        {
            get
            {
                if (_type == null)
                {
                    _type = typeof(TModel);
                }
                return _type;
            }
        }
        
    }

    public class WizardDefinition
    {
        public const string ContextName = "ContextWizard";
        public string Name { get; private set; }
        public List<IWizardStepMeta> Steps = new List<IWizardStepMeta>();

   
        public WizardDefinition(string name, Func<WizardStep, IController> createController = null)
        {
            Name = name;
        }

        public WizardDefinition Add<TModel>(string view, Func<TModel, ActionResult> action = null) where TModel : WizardStep, new()
        {
            Steps.Add(new WizardStepMeta<TModel>(view, this, action));
            return this;
        }


        #region static stuff
        static Dictionary<string, WizardDefinition> _definitions = new Dictionary<string, WizardDefinition>();

        public static WizardDefinition GetDefinition(string name)
        {
            return _definitions[name];
        }

        public static void Register(WizardDefinition def)
        {
            _definitions.Add(def.Name, def);
        }

        #endregion
    }


    public class StatusMessage
    {
        public MessageType MessageType { get; set; }
        public string Title { set; get; }
        public string Message { set; get; }


    }

    public enum MessageType
    {
        Success = 1, Primary = 2, Warning = 3, Danger = 4
    }

    public interface IDatabase
    {

    }

    public class SomeDb: IDatabase { }
}