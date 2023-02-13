// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// 
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

namespace ProxyTray;

public struct ProxySettingsResult
{
	public bool Enabled { get; }
	public string Server { get; }

	public ProxySettingsResult(bool enabled, string server)
	{
		Enabled = enabled;
		Server = server;
	}
}