using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Owin;
using Hangfire;
using HangfireMultipleInstance.Tasks;
using log4net;

namespace HangfireMultipleInstance
{
    public static class HangfireConfiguration
    {
        private const string DefaultQueue = "default";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(HangfireConfiguration));

        private static string[] _environmentQueues;
        private static string[] EnvironmentQueues
        {
            get
            {
                return _environmentQueues ?? (_environmentQueues = GetEnvironmentQueues());
            }
        }

        public static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("main");

            var options = new BackgroundJobServerOptions
            {
                Queues = EnvironmentQueues
            };
            app.UseHangfireServer(options);
            Logger.InfoFormat("Hangire has been configured with queues: '{0}'", string.Join(",", options.Queues));

            app.UseHangfireDashboard();

            RegisterTasks();
        }

        private static void RegisterTasks()
        {
            AddRecurringJob<SyncDatabaseTask>("SyncDatabase", t => t.Sync(), Cron.MinuteInterval(5), GetAppSetting("Hangfire.SyncDatabase.Queue"));
            AddRecurringJob<SendEmailsTask>("SendEmailsTask", t => t.Send(), Cron.MinuteInterval(3), GetAppSetting("Hangfire.SendEmails.Queue"));
            AddRecurringJob<RebuildIndexesTask>(GetAppSetting("Hangfire.RebuildIndexes.Id"), t => t.Rebuild(), Cron.MinuteInterval(10), GetAppSetting("Hangfire.RebuildIndexes.Queue"));
        }

        private static void AddRecurringJob<T>(string id, Expression<Action<T>> method, string cron, string queue = DefaultQueue)
        {
            if (!EnvironmentQueues.Contains(queue, StringComparer.InvariantCultureIgnoreCase))
                return;

            Logger.InfoFormat("Hangfire job '{0}' has been scheduled with cron {1} for queue: '{2}'", id, cron, queue);

            RecurringJob.AddOrUpdate<T>(id, method, cron, null, queue);
        }

        private static string[] GetEnvironmentQueues()
        {
            var queues = GetAppSetting("Hangfire.Queues").Split(',')
                    .Select(q => q.Trim().ToLowerInvariant())
                    .Where(q => !string.IsNullOrEmpty(q))
                    .Distinct()
                    .ToList();

            if (!queues.Any())
            {
                queues.Add(DefaultQueue);
            }

            return queues.ToArray();
        }

        private static string GetAppSetting(string settingKey)
        {
            return ConfigurationManager.AppSettings[settingKey] ?? string.Empty;
        }
    }
}