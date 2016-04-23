using akumaki.Core;
using akumaki.UI;
using System.Windows;

namespace akumaki
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// NotifyIcon in the TaskTray
        /// </summary>
        private NotifyIconWrapper notifyIcon;

        /// <summary>
        /// Raises the Startup event
        /// </summary>
        /// <param name="e">A StartupEventArgs that contains the event data</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            notifyIcon = new NotifyIconWrapper();
            WindowLocationStore.Initialize();
        }

        /// <summary>
        /// Raises the Exit event
        /// </summary>
        /// <param name="e">An ExitEventArgs that contains the event data</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            notifyIcon.Dispose();
        }
    }
}
