using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachManager : IManager
{

	public class InitData : ManagerInitData
	{
		public AppManager m_appManager;

		public InitData( AppManager appManager )
		{
			m_appManager = appManager;
		}
	}

	#region IManager

	public void Setup( ManagerInitData initData ) 
	{
	}

	public void Teardown()
	{
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
}
