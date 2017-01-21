using UnityEngine;
using System.Collections;

public class AppManager : BaseAppManager
{
	private static AppManager _instance;
	public static AppManager Instance { get { return _instance; } }

	private UIManager _uiManager = null;
	public UIManager UIManager { get { return _uiManager; } }

	private ProgressionManager _progressionManager = null;
	public ProgressionManager ProgressionManager { get { return _progressionManager; } }

	private GameStateManager _gameStateManager = null;
	public GameStateManager GameStateManager { get { return _gameStateManager; } }

	#region IManager

	public override void Setup( ManagerInitData initData ) 
	{
		base.OnSetup( initData );

		_uiManager = AddManager<UIManager>( new UIManager.InitData(this) );
		_progressionManager = AddManager<ProgressionManager>( new ProgressionManager.InitData(this) );
		_gameStateManager = AddManager<GameStateManager>( new GameStateManager.InitData(this) );

		_instance = this;
	}

	public override void Teardown()
	{
		_instance = null;
		base.OnTeardown();
	}
	public override void UpdateFrame( float dt )
	{
		base.OnUpdateFrame( dt );
	}
	public override void UpdateFrameLate( float dt )
	{
		base.OnUpdateFrameLate( dt );
	}
	public override void UpdateFixed( float dt )
	{
		base.OnUpdateFixed( dt );
	}

	#endregion
}
