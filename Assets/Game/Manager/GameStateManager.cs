using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : IManager
{
	private UIManager _ui = null;
	private ProgressionManager _prog = null;

	private GameState _gameState = null;
	public GameState GameState { get { return _gameState; } }

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
		AppManager app = (initData as InitData).m_appManager;
		_ui = app.UIManager;
		_prog = app.ProgressionManager;

		_gameState = new GameState();
	}

	public void Teardown()
	{
		_ui = null;
		_prog = null;

		_gameState = null;
	}

	public void UpdateFrame( float dt )
	{
		if( _gameState.m_currentLevel >= 0 && _gameState.m_currentLevel < _prog.GetLevelCount() )
		{
			float oldTime = _gameState.m_levelTime;
			_gameState.m_levelTime += dt;
			float newTime = _gameState.m_levelTime;

			CfgLevel lcfg = _prog.GetLevelConfigByIndex(_gameState.m_currentLevel);

			CoachController.UpdateCoach( _ui, _gameState.m_coach, _gameState.m_rows, lcfg.m_thetaRange, dt );

			List<Wave> newWaves = WaveController.GetNewWavesForTimeRange(lcfg, oldTime, newTime);
			_gameState.m_waves.AddRange( newWaves );

			// update waves!
			bool levelOver = WaveController.UpdateWaves( _gameState.m_waves, dt );
			int failures = CrowdController.UpdateCrowd(_gameState.m_rows, _gameState.m_waves, dt);

			if (failures > 0) 
			{
				Debug.LogWarning ("Losses: " + failures);
			}
		}
	}
	public void UpdateFrameLate( float dt )
	{
	}
	public void UpdateFixed( float dt )
	{
	}
		
	#endregion

	public void NextLevel()
	{
		_gameState.m_currentLevel++;

		CfgLevel lcfg = _prog.GetLevelConfigByIndex(_gameState.m_currentLevel);

		_gameState.m_thetaRange = lcfg.m_thetaRange;
		_gameState.m_levelTime = 0;

		CfgCoach ccfg = _prog.GetCoachConfigById (lcfg.m_coachId);
		CoachController.SetupCoach (_gameState.m_coach, ccfg, lcfg.m_coachPositions );

		_gameState.m_rows = CrowdController.PopulateCrowd(lcfg.m_rows, _prog);

		_gameState.m_waves = WaveController.GetNewWavesForTimeRange(lcfg, -1, 0);

		_ui.OnLevelChange();
	}
}
