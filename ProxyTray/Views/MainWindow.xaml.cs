using System;
using System.Windows;
using System.Windows.Media.Imaging;

using ProxyTray.Monitoring;
using ProxyTray.Proxy;

namespace ProxyTray.Views;

public partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();
		StartMonitor();
		UpdateIcon();
	}

	private void StartMonitor()
	{
		RegistryMonitor.StartMonitor();
		RegistryMonitor.ProxySettingsChanged += OnProxySettingsChanged;
	}

	private void StopMonitor()
	{
		RegistryMonitor.ProxySettingsChanged -= OnProxySettingsChanged;
		RegistryMonitor.StopMonitor();
	}

	private void CheckState()
	{
		var lastProxyState = ProxySettings.LastProxyState;
		if (lastProxyState == ProxyState.Unknown)
			return;

		var proxySettings = ProxySettings.GetProxySettings();
		if (proxySettings.State != lastProxyState)
			ProxySettings.SetProxyState(lastProxyState);
	}

	private void UpdateIcon()
	{
		var proxySettings = ProxySettings.GetProxySettings();

		Dispatcher.BeginInvoke(() =>
		{
			NotificationIcon.ToolTipText = proxySettings.State switch
			{
				ProxyState.Enabled => string.Format(Constants.ProxyEnabledTooltip, proxySettings.Server),
				ProxyState.Disabled => Constants.ProxyDisabledTooltip,
				_ => Constants.ProxyUnknownTooltip
			};

			NotificationIcon.IconSource = (BitmapImage)FindResource
			(
				proxySettings.State switch
				{
					ProxyState.Enabled => Constants.ProxyEnabledIcon,
					ProxyState.Disabled => Constants.ProxyDisabledIcon,
					_ => Constants.ProxyUnknownIcon
				}
			);
		});
	}

	private void ShutdownApplication()
	{
		NotificationIcon.Dispose();
		Application.Current.Shutdown(0);
	}

	private void OnProxySettingsChanged(object? sender, EventArgs e)
	{
		CheckState();
		UpdateIcon();
	}

	private void OnExitMenuItemClick(object? sender, RoutedEventArgs e)
	{
		StopMonitor();
		ShutdownApplication();
	}
}