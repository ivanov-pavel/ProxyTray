using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace ProxyTray
{
    /// <summary>
    ///     Contains methods for retriving and changing the system proxy setting
    /// </summary>
    public static class ProxySettings
    {
        /// <summary>
        ///     Gets the proxy settings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SecurityException">The user does not have the permissions required to read the registry key. </exception>
        /// <exception cref="IOException">
        ///     The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has
        ///     been marked for deletion.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        public static ProxySettingsResult GetProxySettings()
        {
            using (var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey))
            {
                if (internetSettings == null)
                    throw new IOException("Cannot open registry key: " + Constants.RegistryKey);

                var proxyEnabled = (int) internetSettings.GetValue(Constants.ProxyEnabled) == 1;
                var proxyServer = internetSettings.GetValue(Constants.ProxyServer)?.ToString() ?? string.Empty;

                return new ProxySettingsResult(proxyEnabled, proxyServer);
            }
        }

        /// <summary>
        ///     Toggles the proxy.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SecurityException">The user does not have the permissions required to read the registry key.</exception>
        /// <exception cref="IOException">
        ///     The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has
        ///     been marked for deletion.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <exception cref="InvalidOperationException">Cannot turn on proxy if it's not configured</exception>
        public static ProxySettingsResult ToggleProxy()
        {
            var currentSettings = GetProxySettings();
            if (!currentSettings.Enabled && string.IsNullOrEmpty(currentSettings.Server))
                throw new InvalidOperationException("Cannot turn on proxy if it's not configured");

            using (
                var internetSettings = Registry.CurrentUser.OpenSubKey(Constants.RegistryKey,
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.SetValue))
            {
                if (internetSettings == null)
                    throw new IOException("Cannot open registry key: " + Constants.RegistryKey);

                internetSettings.SetValue(Constants.ProxyEnabled, currentSettings.Enabled ? 0 : 1);
                return new ProxySettingsResult(!currentSettings.Enabled, currentSettings.Server);
            }
        }
    }

    /// <summary>
    ///     Contains information about proxy settings
    /// </summary>
    public struct ProxySettingsResult
    {
        /// <summary>
        ///     Gets a value indicating whether proxy is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; }

        /// <summary>
        ///     Gets the proxy server string.
        /// </summary>
        /// <value>
        ///     The proxy server in server:port format.
        /// </value>
        public string Server { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProxySettingsResult" /> struct.
        /// </summary>
        /// <param name="enabled">Whether the proxy is enabled.</param>
        /// <param name="server">The proxy server.</param>
        public ProxySettingsResult(bool enabled, string server)
        {
            Enabled = enabled;
            Server = server;
        }
    }
}