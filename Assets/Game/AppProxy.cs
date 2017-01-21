using UnityEngine;
using System.Collections;

public class AppProxy : BaseAppProxy
{
	[System.Serializable]
	public class Configuration : BaseAppProxy.BaseConfiguration
	{
		[System.Serializable]
		public class Camera
		{
			public UnityEngine.Camera m_mainCamera;
		}
		public Configuration.Camera m_camera;

		[System.Serializable]
		public class GameConfig
		{
			public string[] m_assetBundles;
		}
		public Configuration.GameConfig m_gameConfig;
	}

	public Configuration m_appConfiguration;

	private void Start()
	{
		OnStart<AppManager>( m_appConfiguration );
	}
}
