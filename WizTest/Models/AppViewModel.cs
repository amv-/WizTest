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

    public abstract class WizardBase
    {
        WizardDefinition definition;

        public int Index { set; get; } = 0;

        public WizardStep[] Steps { get; set; }

        public WizardBase()
        {
            definition = WizardDefinition.GetDefinition(this.GetType());
            if(definition == null)
            {
                throw new InvalidOperationException("Wizard type definition not registered");
            }
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
        public static T GetWizardData<T>() where T : WizardBase, new()
        {
            var wizard = HttpContext.Current.Session[WIZ_PREFIX + typeof(T).Name] as T;
            return wizard ?? Reset<T>();
        }

        public static T Reset<T>() where T : WizardBase, new()
        {
            T wizard = new T();
            HttpContext.Current.Session[WIZ_PREFIX + typeof(T).Name] = wizard;
            return wizard;
        }
        #endregion

    }

    public class MyWizard : WizardBase
    {

    }

    public interface IWizardStepMeta
    {
        string View { get;}
        WizardStep Create();
        Type Type {get; }
    }

    public class WizardStepMeta<TModel> : IWizardStepMeta where TModel : WizardStep, new()
    {
        public WizardStepMeta(string view)
        {
            View = view;
        }
        public string View { private set; get; }

        WizardStep IWizardStepMeta.Create()
        {
            return new TModel();
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
        //public Dictionary<string, Type> ViewModelsBindings = new Dictionary<string, Type>();

        public List<IWizardStepMeta> Steps = new List<IWizardStepMeta>();

        public void Add<TModel>(string view) where TModel : WizardStep, new()
        {
            Steps.Add(new WizardStepMeta<TModel>(view));
        }

        #region static stuff
        static Dictionary<Type, WizardDefinition> _stepDefinitions = new Dictionary<Type, WizardDefinition>();

        public static WizardDefinition GetDefinition<T>() where T : WizardBase
        {
            return _stepDefinitions[typeof(T)];
        }

        public static WizardDefinition GetDefinition(Type type)
        {
            return _stepDefinitions[type];
        }

        public static void Register<T>(WizardDefinition def)
        {
            _stepDefinitions.Add(typeof(T), def);
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
}