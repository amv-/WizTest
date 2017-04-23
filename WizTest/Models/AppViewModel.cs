using System;
using System.Collections.Generic;
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

    public abstract class WizardStepModel<T> where T: AppViewModel
    {
        public string View { get; set; }
        public Type ModelType
        {
            get
            {
                return typeof(T);
            }
        }
    }

    public abstract class WizardBase {
        const string WIZ_PREFIX = "Wizard_";

        public WizardBase(params WizardStepModel[] steps)
        {
            if (!steps.Any())
            {
                throw new Exception("No steps defined");
            }
            Steps = new List<WizardStepModel>();
            Steps.AddRange(steps);

        }
        public static T GetWizardData<T>() where T : WizardBase
        {
            return HttpContext.Current.Session[WIZ_PREFIX + typeof(T).Name] as T;
        }       

        public static T Reset<T>() where T: WizardBase, new()
        {
            T wizard = new T();
            HttpContext.Current.Session[WIZ_PREFIX + typeof(T).Name] = wizard;
            return wizard;
        }

        public int Index { set; get; }

        public List<WizardStepModel> Steps { get; protected set; }


    }


    public class MyFirstWizard : WizardBase
    {
        public MyFirstWizard()
        {
            
        }

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