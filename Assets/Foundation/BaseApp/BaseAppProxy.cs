using UnityEngine;
using System.Collections;


public class BaseAppProxy : MonoBehaviour 
{
	[System.Serializable]
	public class BaseConfiguration
	{
		[System.Serializable]
		public class Logs
		{
			[SerializeField]
			[EnumFlagsAttribute]
			public LoggingManager.Domain m_flags;
		}

		public BaseConfiguration.Logs m_logs;

		public string m_assemblyPrimary = "Assembly-CSharp";

		public string[] m_phaseNames;
	}

	private BaseAppManager _appManager;

	#region Monobehavior
	protected void OnStart<T>( BaseConfiguration config ) where T : BaseAppManager, new()
	{
		BaseAppManager.InitData initData = new BaseAppManager.InitData( config, this, transform );
		_appManager = new T();
		_appManager.Setup( initData );
		_appManager.OnPostSetup();
	}

	private void OnDestroy()
	{
		_appManager.Teardown();
		_appManager = null;
	}

	private void Update() 
	{
		_appManager.UpdateFrame( Time.deltaTime );
	}

	private void LateUpdate() 
	{
		_appManager.UpdateFrameLate( Time.deltaTime );
	}

	private void FixedUpdate()
	{
		_appManager.UpdateFixed( Time.fixedDeltaTime );
	}
	#endregion
}
