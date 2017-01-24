﻿using System.Collections;
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

			if (_gameState.m_levelFailedTicker <= 0) 
			{
				List<Wave> newWaves = WaveController.GetNewWavesForTimeRange (lcfg, oldTime, newTime);
				_gameState.m_waves.AddRange (newWaves);

				// update waves!
				WaveController.UpdateWaves (_gameState.m_waves, dt);
				float rightChannel=0;
				float leftChannel=0;
				for (int i = 0; i < _gameState.m_waves.Count; ++i) 
				{
					Wave wave = _gameState.m_waves [i];

					leftChannel = Mathf.Max( leftChannel, Mathf.Max (0, lcfg.m_thetaRange - Mathf.Abs (wave.RawTheta - (180.0f - lcfg.m_thetaRange/2))) / lcfg.m_thetaRange );
					rightChannel = Mathf.Max( rightChannel, Mathf.Max (0, lcfg.m_thetaRange - Mathf.Abs (wave.RawTheta - (180.0f + lcfg.m_thetaRange/2))) / lcfg.m_thetaRange );
						
					float threshold = 180.0f + lcfg.m_thetaRange / 2;
					if (wave.RawTheta >= threshold && wave.RawLastTheta < threshold) 
					{
						int qualityPass = wave.GetAndResetQualityCheck ();
						string effectId = "WaveFinished" + (wave.Invert ? "L" : "R") + qualityPass.ToString ();
						_ui.PlayEffect (effectId);
						_gameState.m_waveIndex++;
					}
				}

				AudioSource asrc = GameObject.Find ("CrowdRoar").GetComponent<AudioSource> ();

				if (leftChannel <= 0 && rightChannel <= 0) {
					if( asrc.isPlaying ) asrc.Stop ();
				} else {
					asrc.volume = Mathf.Min(1.0f, leftChannel + rightChannel);
					asrc.panStereo = rightChannel - leftChannel;

					if (!asrc.isPlaying) asrc.Play ();
				}
			}
			else
			{
				AudioSource asrc = GameObject.Find ("CrowdRoar").GetComponent<AudioSource> ();
				if( asrc.isPlaying ) asrc.Stop ();
			}

			// update crowd
			KeyValuePair<int, int> crowdResult = CrowdController.UpdateCrowd(_gameState, dt);
			_gameState.m_score += crowdResult.Key;
			int failures = crowdResult.Value;

			if (failures > 0) 
			{
				_gameState.m_health = Mathf.Max( 0, _gameState.m_health - failures );
			}

			if (_gameState.m_health <= 0) 
			{
				_gameState.m_levelFailedTicker += dt;	
			}

			bool levelFinished = CheckLevelComplete ();
			if (levelFinished && _gameState.m_levelFailedTicker <= 0) 
			{
				if (_gameState.m_levelFinishedTicker <= 0) 
				{
					_ui.PlayEffect ("Confetti");

					UIHubGameFlier flier = GameObject.Find ("WinSprite").GetComponent<UIHubGameFlier> ();

					if (_gameState.m_currentLevel == _prog.GetLevelCount () - 1) {
						flier.SwapTexture (null);
					} else {
						flier.SwapTexture (_gameState.m_generalConfig.m_beatLevelTextures[Random.Range(0, _gameState.m_generalConfig.m_beatLevelTextures.Length)]);
					}
				}
				_gameState.m_levelFinishedTicker += dt;
			}

			_gameState.m_levelFadeInTicker = Mathf.Max( 0.0f, _gameState.m_levelFadeInTicker - dt );

			if (_gameState.m_levelFinishedTicker > _gameState.m_generalConfig.m_winSavorDelay - _gameState.m_generalConfig.m_levelScrollTime && _gameState.m_currentLevel < _prog.GetLevelCount()-1 )
			{
				NextLevel ();
			}

			CoachController.UpdateCoach( _ui, _gameState, lcfg.m_thetaRange, _prog.GetLevelCount()-1, dt );
		}
	}
	public void UpdateFrameLate( float dt )
	{
	}
	public void UpdateFixed( float dt )
	{
	}
		
	#endregion

	public void Init( CfgGeneral cfgGeneral )
	{
		_gameState.m_generalConfig = cfgGeneral;
		_gameState.m_currentLevel = cfgGeneral.m_startLevel - 1;
		NextLevel ();
	}

	private bool CheckLevelComplete()
	{
		if (_gameState.m_waves.Count == 0) 
		{
			CfgLevel lcfg = _prog.GetLevelConfigByIndex(_gameState.m_currentLevel);
			bool done = true;
			for (int i = 0; i < lcfg.m_waveEntries.Count; ++i) 
			{
				CfgWaveEntry wave = lcfg.m_waveEntries [i];
				if (_gameState.m_levelTime < wave.m_startTime) 
				{
					done = false;
				}
			}
			return done;
		}
		return false;
	}

	public void NextLevel()
	{
		_gameState.m_currentLevel++;

		CfgLevel lcfg = _prog.GetLevelConfigByIndex( _gameState.m_currentLevel);

		AudioSource aSrc = Camera.main.GetComponent<AudioSource> ();
		aSrc.clip = lcfg.m_music;
		aSrc.Play ();

		_gameState.m_thetaRange = lcfg.m_thetaRange;

		_gameState.ResetLevel ();

		CfgCoach ccfg = _prog.GetCoachConfigById (lcfg.m_coachId);
		CoachController.SetupCoach (_gameState.m_currentLevel == 0, _gameState.m_coach, ccfg, lcfg.m_coachPositions );

		_gameState.m_rows = CrowdController.PopulateCrowd(lcfg.m_rows, _prog);

		_gameState.m_waves = WaveController.GetNewWavesForTimeRange(lcfg, -1, 0);
		_gameState.m_waveCount = 0;
		for (int i = 0; i < lcfg.m_waveEntries.Count; ++i) 
		{
			_gameState.m_waveCount += lcfg.m_waveEntries [i].m_iterations;
		}
		_gameState.m_waveIndex = 0;

		_ui.OnLevelChange();

		if( _gameState.m_currentLevel > 0 )
		{
			_gameState.m_levelFadeInTicker = _gameState.m_generalConfig.m_levelScrollTime;
		}
	}
}
