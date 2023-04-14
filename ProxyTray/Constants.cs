namespace ProxyTray;

public static class Constants
{
	internal const string AppGuid = "04052ac4-0332-462d-91ed-c422446d1573";

	internal const string RegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
	internal const string ProxyEnabled = "ProxyEnable";
	internal const string ProxyServer = "ProxyServer";

	internal const string ProxyEnabledIcon = "IconYes";
	internal const string ProxyDisabledIcon = "IconNo";
	internal const string ProxyUnknownIcon = "IconUnknown";
	internal const string ProxyEnabledTooltip = "Proxy enabled {0}";
	internal const string ProxyDisabledTooltip = "No proxy";
	internal const string ProxyUnknownTooltip = "Unknown";
}