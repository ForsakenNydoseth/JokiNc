using System;
using JGeneral.IO.Threading;

namespace JokiNc.Core.Processing
{
    public static class BackgroundTask
    {
        static BackgroundTask()
        {
            SyncThread = new SyncThread<Func<string, object>>();
        }

        private static SyncThread<Func<string, object>> SyncThread
        {
            get;
        }

        public static void Place(Func<string, object> method) => SyncThread.TryExecuteItem(method);

        public static void Place(Func<string, object> method, Action<object> resultProcessing)
        {
            Message<Func<string, object>> a = new Message<Func<string, object>>(param =>
            {
                var result = method.Invoke(param);
                resultProcessing.Invoke(result);

                return null;
            });
            SyncThread.TryExecuteItem(a);
        }
    }
}