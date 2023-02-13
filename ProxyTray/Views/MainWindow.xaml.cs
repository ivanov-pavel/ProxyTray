using System.Management;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProxyTray;

public partial class MainWindow
{
	private ManagementEventWatcher? _managementEventWatcher;

	public MainWindow()
	{
		InitializeComponent();
		StartMonitor();
		UpdateIcon();
	}

	private void StartMonitor()
	{
		var currentUser = WindowsIdentity.GetCurrent();
		if (currentUser.User == null)
		{
			MessageBox.Show("Error obtaining current user Windows identity.", "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
			Application.Current.Shutdown(-1);
			return;
		}

		var query = new WqlEventQuery
		(
			$@"SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{currentUser.User.Value}\\{Constants.RegistryKey.Replace(@"\", @"\\")}' AND ValueName='{Constants.ProxyEnabled}'"
		);

		_managementEventWatcher = new ManagementEventWatcher(query);
		_managementEventWatcher.EventArrived += (_, _) => UpdateIcon();
		_managementEventWatcher.Start();
	}

	private void UpdateIcon()
	{
		var settings = ProxySettings.GetProxySettings();

		Dispatcher.BeginInvoke(() =>
		{
			NotificationIcon.ToolTipText = settings.Enabled
				? string.Format(Constants.ProxyEnabledTooltip, settings.Server)
				: Constants.ProxyDisabledTooltip;

			NotificationIcon.IconSource = (BitmapImage)FindResource(settings.Enabled ? Constants.ProxyEnabledIcon : Constants.ProxyDisabledIcon);
		});
	}

	private void OnExitMenuItemClick(object sender, RoutedEventArgs e)
	{
		if (_managementEventWatcher is not null)
		{
			_managementEventWatcher.Stop();
			_managementEventWatcher.Dispose();
		}

		NotificationIcon.Dispose();
		Application.Current.Shutdown(0);
	}
}