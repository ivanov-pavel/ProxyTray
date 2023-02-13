using System;
using System.Threading;
using System.Windows;

namespace ProxyTray;

static class Program
{
	[STAThread]
	public static void Main()
	{
		using var mutex = new Mutex(false, Constants.AppGuid);
		if (!mutex.WaitOne(0, false))
		{
			MessageBox.Show("Instance already running");
			return;
		}

		var app = new App();
		app.InitializeComponent();
		app.Run();
	}
}
