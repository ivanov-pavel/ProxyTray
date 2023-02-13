using System;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace ProxyTray;

public static class ProxySettings
{
	public static ProxySettingsResult GetProxySettings()
	{
		using var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey);
		if (internetSettings == null)
			throw new IOException($"Cannot open registry key: {Constants.RegistryKey}");

		var proxyEnabled = (int?)internetSettings.GetValue(Constants.ProxyEnabled) == 1;
		var proxyServer = internetSettings.GetValue(Constants.ProxyServer)?.ToString() ?? string.Empty;

		return new ProxySettingsResult(proxyEnabled, proxyServer);
	}

	public static ProxySettingsResult ToggleProxy()
	{
		var currentSettings = GetProxySettings();
		if (!currentSettings.Enabled && string.IsNullOrEmpty(currentSettings.Server))
			throw new InvalidOperationException("Cannot turn on proxy if it's not configured");

		using var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.SetValue);
		if (internetSettings == null)
			throw new IOException($"Cannot open registry key: {Constants.RegistryKey}");

		internetSettings.SetValue(Constants.ProxyEnabled, currentSettings.Enabled ? 0 : 1);
		return new ProxySettingsResult(!currentSettings.Enabled, currentSettings.Server);
	}
}