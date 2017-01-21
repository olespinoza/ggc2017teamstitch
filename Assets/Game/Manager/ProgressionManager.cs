using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : IManager
{
	private Dictionary< string, CfgCrowdConfig > _crowdConfigs = new Dictionary<string, CfgCrowdConfig>();
	private List< CfgLevel > _levels = new List<CfgLevel>();

	public class InitData : ManagerInitData
	{
		public string[] m_assetBundleList;

		public InitData( AppManager appManager )
		{
			AppProxy.Configuration appConfig = appManager.AppProxyConfig as AppProxy.Configuration;
			
			m_assetBundleList = appConfig.m_gameConfig.m_assetBundles;
		}
	}

	#region IManager

	public int GetLevelCount()
	{
		return _levels.Count;
	}
	public CfgLevel GetLevelConfigByIndex( int index )
	{
		return _levels[index];
	}

	public CfgCrowdConfig GetCrowdConfigById( string id )
	{
		return _crowdConfigs[id];
	}

	public void Setup( ManagerInitData initData ) 
	{
		InitData data = initData as InitData;

		for( int i=0; i<data.m_assetBundleList.Length; ++i )
		{
			string assetBundleId = data.m_assetBundleList[i];
			Object resource = Resources.Load( assetBundleId );

			if( resource is ConfigBundle )
			{
				ParseConfigBundle( resource as ConfigBundle );
			}
		}

		Debug.Log("Loaded " + _levels.Count.ToString() + " levels.");
	}

	public void Teardown()
	{
		_crowdConfigs = null;
		_levels = null;
	}
	public void UpdateFrame( float dt )
	{
	}
	public void UpdateFrameLate( float dt )
	{
	}
	public void UpdateFixed( float dt )
	{
	}

	#endregion

	void ParseConfigBundle( ConfigBundle bundle )
	{
		for( int i=0; i<bundle.m_levels.Count; ++i )
		{
			CfgLevel lcfg = bundle.m_levels[i];
			_levels.Add( lcfg );
		}
		for( int i=0; i<bundle.m_crowdConfigs.Count; ++i )
		{
			CfgCrowdConfig ccfg = bundle.m_crowdConfigs[i];
			if( _crowdConfigs.ContainsKey( ccfg.m_id ) )
			{
				Debug.LogWarning("Multiple Crowd Entries found for ID: " + ccfg.m_id);
			}
			else
			{
				_crowdConfigs[ ccfg.m_id ] = ccfg;
			}
		}
	}
}
