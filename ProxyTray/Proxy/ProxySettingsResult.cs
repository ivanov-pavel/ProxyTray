﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// 
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

namespace ProxyTray.Proxy;

public struct ProxySettingsResult
{
	public ProxyState State { get; }
	public string Server { get; }

	public ProxySettingsResult(ProxyState state, string server)
	{
		State = state;
		Server = server;
	}
}