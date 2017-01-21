using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseAppManager : IManager
{
	public sealed class InitData : ManagerInitData
	{
		public BaseAppProxy.BaseConfiguration m_appProxyConfig;
		public MonoBehaviour m_coroutineRunner;
		public Transform m_rootTransform;

		public InitData( BaseAppProxy.BaseConfiguration appProxyConfig, MonoBehaviour coroutineRunner, Transform rootTransform )
		{
			m_appProxyConfig = appProxyConfig;
			m_coroutineRunner = coroutineRunner;
			m_rootTransform = rootTransform;
		}
	}

	private BaseAppProxy.BaseConfiguration _appProxyConfig = null;
	public BaseAppProxy.BaseConfiguration AppProxyConfig { get { return _appProxyConfig; } }
	private MonoBehaviour _corountineRunner = null;
	public MonoBehaviour CoroutineRunner { get { return _corountineRunner; } }
	private Transform _rootTransform = null;
	public Transform RootTransform { get { return _rootTransform; } }

	private List<IManager> _managers = null;
	private List<IPhase> _phases = null;

	private LoggingManager _loggingManager = null;
	public LoggingManager Loggingmanager { get { return _loggingManager; } }

	#region IManager

	public abstract void Setup( ManagerInitData initData );
	public abstract void Teardown();
	public abstract void UpdateFrame( float dt );
	public abstract void UpdateFrameLate( float dt );
	public abstract void UpdateFixed( float dt );

	#endregion

	protected void OnSetup( ManagerInitData initData )
	{
		InitData id = initData as InitData;
		_appProxyConfig = id.m_appProxyConfig;
		_corountineRunner = id.m_coroutineRunner;
		_rootTransform = id.m_rootTransform;

		_managers = new List<IManager>();
		_phases = new List<IPhase>();

		_loggingManager = AddManager<LoggingManager>( new LoggingManager.InitData(this) );
	}

	public void OnPostSetup()
	{
		// push initial phase
		PushPhase( _appProxyConfig.m_phaseNames[0] );
	}

	protected void OnTeardown()
	{
		_corountineRunner.StopAllCoroutines();

		PopPhaseAll();
		ClearManagers();

		_loggingManager = null;
	}

	protected void OnUpdateFrame( float dt )
	{
		for( int i=0; i<_managers.Count; ++i )
		{
			_managers[i].UpdateFrame( dt );
		}

		int phIndex = _phases.Count-1;
		if( _phases[phIndex].UpdateFrame( dt ) )
		{
			PushPhase( _appProxyConfig.m_phaseNames[phIndex + 1] );
		}
	}

	protected void OnUpdateFrameLate( float dt )
	{
		for( int i=0; i<_managers.Count; ++i )
		{
			_managers[i].UpdateFrameLate( dt );
		}
	}

	protected void OnUpdateFixed( float dt )
	{
		for( int i=0; i<_managers.Count; ++i )
		{
			_managers[i].UpdateFixed( dt );
		}
	}

	protected T AddManager<T>( ManagerInitData initData ) where T:IManager, new()
	{
		T manager = new T();
		manager.Setup(initData);
		_managers.Add( manager );
		return manager;
	}

	private void ClearManagers()
	{
		for( int i=0; i<_managers.Count; ++i )
		{
			_managers[i].Teardown();
			_managers[i] = null;
		}
		_managers.Clear();
	}

	private void PushPhase( string phaseID )
	{
		System.Runtime.Remoting.ObjectHandle handle = System.Activator.CreateInstance( _appProxyConfig.m_assemblyPrimary, phaseID );
		IPhase phase = handle.Unwrap() as IPhase;

		if( LoggingManager.Instance.Filter( LoggingManager.Domain.Phases ) )
		{
			Debug.Log( "(AppManager) Entering phase: " + phaseID + " (" + _phases.Count + ")" );
		}
		_phases.Add( phase );
		phase.Setup( this );
	}

	private void PopPhase()
	{
		Debug.Assert( _phases.Count > 0, "Attempt to pop past last phase" );
		int phIndex = _phases.Count-1;
		if( LoggingManager.Instance.Filter( LoggingManager.Domain.Phases ) )
		{
			Debug.Log( "(AppManager) Leaving phase: " + phIndex );
		}
		_phases[phIndex].Teardown();
		_phases.RemoveAt( phIndex );
	}

	private void PopPhaseAll()
	{
		while( _phases.Count > 0 )
		{
			PopPhase();
		}
	}
}
