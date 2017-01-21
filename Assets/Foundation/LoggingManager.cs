using UnityEngine;
using System.Collections;

public class LoggingManager : IManager
{
	public enum Domain
	{
		Phases = 			(1<<0),
		CameraTransitions = (1<<1),
		GameView = 			(1<<2),
	}

	public class InitData : ManagerInitData
	{
		public BaseAppManager m_appManager;

		public InitData( BaseAppManager appManager )
		{
			m_appManager = appManager;
		}
	}

	private static LoggingManager _instance = null;
	public static LoggingManager Instance { get { return _instance; } }

	private AppProxy.Configuration.Logs _configData;

	#region IManager

	public void Setup( ManagerInitData initData ) 
	{
		InitData id = initData as InitData;

		_configData = id.m_appManager.AppProxyConfig.m_logs;
		_instance = this;

		Application.logMessageReceived += HandleLog;
	}

	public void Teardown() 
	{
		Application.logMessageReceived -= HandleLog;
		_instance = null;
	}

	public void UpdateFrame( float dt )
	{
	}

	public void UpdateFixed( float dt )
	{
	}

	public void UpdateFrameLate( float dt )
	{
	}

	public bool Filter( Domain domainFlag )
	{
		return (_configData.m_flags & domainFlag) != 0;
	}
	#endregion

	public void HandleLog( string logString, string stackTrace, UnityEngine.LogType type )
	{
	}
}
