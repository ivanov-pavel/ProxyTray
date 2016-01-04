using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace ProxyTray
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// The application unique identifier
        /// </summary>
        public const string AppGuid = "04052ac4-0332-462d-91ed-c422446d1573";

        /// <summary>
        ///     Application Entry Point.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">
        ///     The named mutex exists and has access control security, but the user does
        ///     not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.
        /// </exception>
        /// <exception cref="IOException">A Win32 error occurred.</exception>
        /// <exception cref="WaitHandleCannotBeOpenedException">
        ///     The named mutex cannot be created, perhaps because a wait handle of
        ///     a different type has the same name.
        /// </exception>
        /// <exception cref="AbandonedMutexException">
        ///     The wait completed because a thread exited without releasing a mutex. This
        ///     exception is not thrown on Windows 98 or Windows Millennium Edition.
        /// </exception>
        [STAThread]
        public static void Main()
        {
            using (var mutex = new Mutex(false, AppGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Instance already running");
                    return;
                }

                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}