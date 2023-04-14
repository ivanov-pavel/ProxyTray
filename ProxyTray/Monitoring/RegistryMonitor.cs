// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

using System;
using System.Management;
using System.Security.Principal;
using System.Windows;

namespace ProxyTray.Monitoring;

public static class RegistryMonitor
{
	private static ManagementEventWatcher? _managementEventWatcher;

	public static event EventHandler? ProxySettingsChanged;

	public static void StartMonitor()
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
		_managementEventWatcher.EventArrived += OnProxySettingsChanged;
		_managementEventWatcher.Start();
	}

	public static void StopMonitor()
	{
		if (_managementEventWatcher is null)
			return;

		_managementEventWatcher.EventArrived -= OnProxySettingsChanged;
		_managementEventWatcher.Stop();
		_managementEventWatcher.Dispose();
		_managementEventWatcher = null;
	}

	private static void OnProxySettingsChanged(object sender, EventArrivedEventArgs e)
	{
		ProxySettingsChanged?.Invoke(null, EventArgs.Empty);
	}
}