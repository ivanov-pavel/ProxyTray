using System;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProxyTray
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ManagementEventWatcher _managementEventWatcher;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            StartMonitor();
            UpdateIcon();
        }

        private void StartMonitor()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            if (currentUser?.User == null)
            {
                MessageBox.Show("Error obtaining current user Windows identity.", "Fatal error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Application.Current.Shutdown(-1);
            }
            Debug.Assert(currentUser != null, "currentUser != null");
            Debug.Assert(currentUser.User != null, "currentUser.User != null");

            var query = new WqlEventQuery(
                $@"SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{currentUser.User.Value}\\{Constants
                    .RegistryKey.Replace(@"\", @"\\")}' AND ValueName='{Constants.ProxyEnabled}'");
            _managementEventWatcher = new ManagementEventWatcher(query);
            _managementEventWatcher.EventArrived += (sender, args) => UpdateIcon();
            _managementEventWatcher.Start();
        }

        private void UpdateIcon()
        {
            var settings = ProxySettings.GetProxySettings();

            Dispatcher.BeginInvoke((Action) (() =>
            {
                NotificationIcon.ToolTipText = settings.Enabled
                    ? string.Format(Constants.ProxyEnabledTooltip, settings.Server)
                    : Constants.ProxyDisabledTooltip;

                NotificationIcon.IconSource =
                    (BitmapImage)
                        FindResource(settings.Enabled ? Constants.ProxyEnabledIcon : Constants.ProxyDisabledIcon);
            }));
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _managementEventWatcher.Stop();
            _managementEventWatcher.Dispose();
            NotificationIcon.Dispose();
            Application.Current.Shutdown(0);
        }
    }
}