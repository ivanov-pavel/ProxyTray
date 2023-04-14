using System;
using System.Windows;
using System.Windows.Input;

using ProxyTray.Proxy;

namespace ProxyTray.Commands;

public class ToggleProxyCommand : ICommand
{
	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public void Execute(object? parameter)
	{
		try
		{
			ProxySettings.ToggleProxyState();
		}
		catch (InvalidOperationException exception)
		{
			MessageBox.Show($"Cannot toggle the proxy: {exception.Message}", "Cannot toggle the proxy", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public event EventHandler? CanExecuteChanged;
}