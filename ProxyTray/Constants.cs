namespace ProxyTray
{
    /// <summary>
    ///     Contains constants for the application
    /// </summary>
    public class Constants
    {
        internal const string RegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
        internal const string ProxyEnabled = "ProxyEnable";
        internal const string ProxyServer = "ProxyServer";

        internal const string ProxyEnabledTooltip = "Proxy: {0}";
        internal const string ProxyEnabledIcon = "IconYes";
        internal const string ProxyDisabledTooltip = "No proxy";
        internal const string ProxyDisabledIcon = "IconNo";
    }
}