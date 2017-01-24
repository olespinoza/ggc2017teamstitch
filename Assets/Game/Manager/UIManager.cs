using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHub
{
	void OnLevelChange( AppManager app );
	void UI( AppManager app );
}

public class UIManager : IManager
{
	public static readonly float STADIUM_WIDTH = 900.0f;

	public static readonly float CHARACTER_WIDTH = 158.0f;
	public static readonly float CHARACTER_HEIGHT = 256.0f;

	public static readonly float COACH_WIDTH = 256.0f;
	public static readonly float COACH_HEIGHT = 256.0f;

	private AppManager _app = null;
	private List<IHub> _globalHubs = null;

	private bool _skipNextFrame = false;

	public bool IsSkippingNextFrame { get { return _skipNextFrame; } }

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
		InitData data = initData as InitData;
		_app = data.m_appManager;
		
		_globalHubs = new List<IHub>();
		MonoBehaviour[] allMonobehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
		foreach( MonoBehaviour mb in allMonobehaviours )
		{
			if( mb is IHub )
			{
				_globalHubs.Add( mb as IHub );
			}
		}
	}

	public void Teardown()
	{
		_globalHubs.Clear();
		_app = null;
	}

	public void UpdateFrame( float dt )
	{
		if( !_skipNextFrame )
		{
			for( int i=0; i<_globalHubs.Count; ++i )
			{
				_globalHubs[i].UI( _app );
			}
		}
		_skipNextFrame = false;
	}
	public void UpdateFrameLate( float dt )
	{
	}
	public void UpdateFixed( float dt )
	{
	}

	#endregion

	public void OnLevelChange()
	{
		for( int i=0; i<_globalHubs.Count; ++i )
		{
			_globalHubs[i].OnLevelChange(_app);
		}
		_skipNextFrame = true;
	}

	public void PlayEffect( string effectId )
	{
		for (int i = 0; i < _globalHubs.Count; ++i) 
		{
			if( _globalHubs[i] is UIHubMovieEffect )
			{
				UIHubMovieEffect hme = _globalHubs [i] as UIHubMovieEffect;
				if (hme.name == effectId)
				{
					hme.Play ();
				}
			}
		}
	}
}
