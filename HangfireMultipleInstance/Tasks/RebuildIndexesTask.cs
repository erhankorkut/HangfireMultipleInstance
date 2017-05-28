using System.Threading;
using log4net;

namespace HangfireMultipleInstance.Tasks
{
    public class RebuildIndexesTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RebuildIndexesTask));

        public void Rebuild()
        {
            Logger.Info("RebuildIndexes started");
            Thread.Sleep(30000);
            Logger.Info("RebuildIndexes finished");
        }
    }
}