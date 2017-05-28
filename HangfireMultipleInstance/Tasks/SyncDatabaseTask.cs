using System.Threading;
using log4net;

namespace HangfireMultipleInstance.Tasks
{
    public class SyncDatabaseTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SyncDatabaseTask));

        public void Sync()
        {
            Logger.Info("SyncDatabase started");
            Thread.Sleep(10000);
            Logger.Info("SyncDatabase finished");
        }
    }
}