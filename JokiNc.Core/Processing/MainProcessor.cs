using System;
using System.Runtime.InteropServices;
using JGeneral.IO.Threading;
using JokiNc.Core.Processing.DefaultActions;
using JokiNc.Core.UnityCore;

namespace JokiNc.Core.Processing
{
    public static class MainProcessor
    {
        static MainProcessor()
        {
            SyncThread = new SyncThread<Action>();
        }
        
        public static SyncThread<Action> SyncThread
        {
            get;
        }

        public static void EnQueue(this LineElement element)
        {
            SyncThread.TryExecuteItem(() =>
            {
                Defaults.DefaultMapping[element.Content](element);
            });
        }
    }
}