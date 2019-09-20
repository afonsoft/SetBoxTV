using System;
using System.Threading.Tasks;
using Rollbar;
using Rollbar.DTOs;
using Rollbar.Telemetry;

namespace VideoPlayerProima.Helpers
{
    /// <summary>
    /// Class RollbarHelper.
    /// A utility class aiding in Rollbar SDK usage.
    /// </summary>
    public static class RollbarHelper
    {
        public static readonly TimeSpan RollbarTimeout = TimeSpan.FromSeconds(10);
        public const string rollbarAccessToken = "a2b1e541b25947cab3b00c956ded3535";
        public const string rollbarEnvironment = "production";
        
        /// <summary>
        /// Registers for global exception handling.
        /// </summary>
        public static void RegisterForGlobalExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var newExc = new System.Exception("CurrentDomainOnUnhandledException", args.ExceptionObject as System.Exception);
                RollbarLocator.RollbarInstance.AsBlockingLogger(RollbarTimeout).Critical(newExc);
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var newExc = new ApplicationException("TaskSchedulerOnUnobservedTaskException", args.Exception);
                RollbarLocator.RollbarInstance.AsBlockingLogger(RollbarTimeout).Critical(newExc);
            };
        }

        /// <summary>
        /// Configures the rollbar telemetry.
        /// </summary>
        private static void ConfigureRollbarTelemetry()
        {
            TelemetryConfig telemetryConfig = new TelemetryConfig(
                telemetryEnabled: true,
                telemetryQueueDepth: 3
            );
            TelemetryCollector.Instance.Config.Reconfigure(telemetryConfig);
        }

        /// <summary>
        /// Configures the Rollbar singleton-like notifier.
        /// </summary>
        public static void ConfigureRollbarSingleton()
        {

            var Config = new RollbarConfig(rollbarAccessToken)
            {
                Environment = rollbarEnvironment,
                ScrubFields = new[] { "access_token", "Username" }
            };

            // minimally required Rollbar configuration:
            RollbarLocator.RollbarInstance.Configure(Config);

            ConfigureRollbarTelemetry();

            // Optional info about reporting Rollbar user:
            SetRollbarReportingUser("001", "afonsoft@gmail.com", "afonsoft");
        }

        /// <summary>
        /// Sets the rollbar reporting user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="userName">Name of the user.</param>
        private static void SetRollbarReportingUser(string id, string email, string userName)
        {
            Person person = new Person(id) {Email = email, UserName = userName};
            RollbarLocator.RollbarInstance.Config.Person = person;
        }

    }
}
