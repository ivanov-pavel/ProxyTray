using System;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace ProxyTray
{
    /// <summary>
    ///     Toggles the proxy on/off
    /// </summary>
    public class ToggleProxyCommand : ICommand
    {
        #region Implementation of ICommand

        /// <summary>
        ///     Determines whether this instance can execute the specified parameter. Always returns true.
        /// </summary>
        /// <param name="parameter">The parameter (ignored).</param>
        /// <returns>
        ///     <c>true</c>
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///     Executes the specified parameter, toggling the proxy on/off
        /// </summary>
        /// <param name="parameter">The parameter (ignored).</param>
        /// <exception cref="SecurityException">The user does not have the permissions required to read the registry key.</exception>
        /// <exception cref="IOException">
        ///     The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has
        ///     been marked for deletion.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        public void Execute(object parameter)
        {
            try
            {
                ProxySettings.ToggleProxy();
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show("Cannot toggle the proxy: " + ioe.Message, "Cannot toggle the proxy",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <remarks>
        ///     Is never fired
        /// </remarks>
        public event EventHandler CanExecuteChanged;

        #endregion
    }
}