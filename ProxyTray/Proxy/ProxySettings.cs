using System;
using System.IO;
using System.Security.AccessControl;

using Microsoft.Win32;

namespace ProxyTray.Proxy;

public static class ProxySettings
{
	public static ProxyState LastProxyState { get; private set; } = ProxyState.Unknown;

	public static ProxySettingsResult GetProxySettings()
	{
		using var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey);
		if (internetSettings == null)
			throw new IOException($"Cannot open registry key: {Constants.RegistryKey}");

		var proxyState = internetSettings.GetValue(Constants.ProxyEnabled) switch
		{
			0 => ProxyState.Disabled,
			1 => ProxyState.Enabled,
			_ => ProxyState.Unknown
		};
		var proxyServer = internetSettings.GetValue(Constants.ProxyServer)?.ToString() ?? string.Empty;

		return new ProxySettingsResult(proxyState, proxyServer);
	}

	public static void SetProxyState(ProxyState proxyState)
	{
		using var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.SetValue);
		if (internetSettings == null)
			throw new IOException($"Cannot open registry key: {Constants.RegistryKey}");

		internetSettings.SetValue(Constants.ProxyEnabled, proxyState == ProxyState.Enabled ? 1 : 0);
		LastProxyState = proxyState;
	}

	public static void ToggleProxyState()
	{
		var proxySettings = GetProxySettings();

		if (proxySettings.State == ProxyState.Unknown)
			throw new InvalidOperationException("Proxy state is unknown!");

		if (proxySettings.State == ProxyState.Disabled && string.IsNullOrEmpty(proxySettings.Server))
			throw new InvalidOperationException("Cannot turn on proxy if it's not configured");

		SetProxyState(proxySettings.State == ProxyState.Enabled ? ProxyState.Disabled : ProxyState.Enabled);
	}
}