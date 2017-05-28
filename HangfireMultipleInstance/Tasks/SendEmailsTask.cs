using System.Threading;
using log4net;

namespace HangfireMultipleInstance.Tasks
{
    public class SendEmailsTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SendEmailsTask));

        public void Send()
        {
            Logger.Info("SendEmails started");
            Thread.Sleep(5000);
            Logger.Info("SendEmails finished");
        }
    }
}