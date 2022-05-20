using System;
using JokiNc.Core.UnityCore;

namespace JokiNc.Core
{
    public class Status
    {
        public static Status Instance;
        public ToolOptions Options { get; }
        public WorkingObjectOptions WorkObjectOptions { get; }
        public string Message { get; private set; }
        
        public Status()
        {
            if (Instance is not null)
            {
                throw new Exception("Cannot create more than one instance of class 'Status'!");
            }

            if (ToolController.Instance is null)
            {
                throw new Exception("Cannot create instance of class 'Status' if the class 'ToolController' has not been instantiated yet!");
            }

            Options = ToolController.Instance.Options;
            WorkObjectOptions = ToolController.Instance.WorkingObjOptions;
            Message = "Status has been Instantiated!";
        }

        public virtual void Update(){}

        public void SetMessage(string message)
        {
            Message = message;
        }
    }
}