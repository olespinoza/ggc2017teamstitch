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

			CfgLevel lcfg = _prog.GetLevelConfigByIndex(_gameState.m_currentLevel);
			for( int i=0; i<lcfg.m_waveEntries.Count; ++i )
			{
				CfgWaveEntry wave = lcfg.m_waveEntries[i];
				if( oldTime < wave.m_startTime && _gameState.m_levelTime > wave.m_startTime )
				{
					_gameState.m_waves.Add( new Wave( wave ) );
				}
			}

			// update waves!
			for( int i=0; i<_gameState.m_waves.Count; ++i )
			{
				Wave wave = _gameState.m_waves[i];
				if( wave.Update( dt ) )
				{
					_gameState.m_waves.RemoveAt(i);
					--i;
				}
			}

			// update characters
			for( int i=0; i<_gameState.m_rows.Count; ++i )
			{
				CrowdRow row = _gameState.m_rows[i];
				for( int j=0; j<row.m_individuals.Count; ++j )
				{
					CrowdIndividual individual = row.m_individuals[j];

					float amplitude = 0;
					for( int k=0; k<_gameState.m_waves.Count; ++k )
					{
						Wave wave = _gameState.m_waves[k];
						float waveMag = wave.GetMagnitudeAt( individual.m_waveTheta );
						amplitude = Mathf.Max( amplitude, waveMag );
					}

					// fail
					if( amplitude > 0 && individual.GetState() == CrowdIndividual.State.STATE_SITTING )
					{
						Debug.LogWarning("YOU LOOOSE");
					}
					// start/continue wave
					else
					{
						individual.m_waveAmount = amplitude;
					}
				}
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
		_gameState.m_waves.Clear();
		_gameState.m_rows.Clear();

		_gameState.m_currentLevel++;


		CfgLevel lcfg = _prog.GetLevelConfigByIndex(_gameState.m_currentLevel);

		_gameState.m_thetaRange = lcfg.m_thetaRange;
		_gameState.m_levelTime = 0;

		List<CfgCrowdEntry> entries = lcfg.m_crowdEntries;

		_gameState.m_rows.Clear();
		for( int i=0; i<lcfg.m_rows.Length; ++i )
		{
			CrowdRow row = new CrowdRow();

			int seats = lcfg.m_rows[i].m_seats;
			for( int j=0; j<seats; ++j )
			{
				CrowdIndividual individual = new CrowdIndividual();

				CfgCrowdEntry cecfg = RollCrowdEntry( entries, i+1 );
				CfgCrowdConfig cccfg = _prog.GetCrowdConfigById( cecfg.m_crowdConfigId );
				individual.Setup( cccfg );
				row.m_individuals.Add( individual );
			}

			_gameState.m_rows.Add( row );
		}

		for( int i=0; i<lcfg.m_waveEntries.Count; ++i )
		{
			CfgWaveEntry wave = lcfg.m_waveEntries[i];
			if( wave.m_startTime == 0 )
			{
				_gameState.m_waves.Add( new Wave( wave ) );
			}
		}

		_ui.OnLevelChange();
	}

	public CfgCrowdEntry RollCrowdEntry( List<CfgCrowdEntry> entries, int row )
	{
		List< KeyValuePair<int,int> > weights = new List< KeyValuePair<int,int> >();
		int totalWeight = 0;
		for( int i=0; i<entries.Count; ++i )
		{
			CfgCrowdEntry entry = entries[i];
			if( row >= entry.m_minRow && row <= entry.m_maxRow )
			{
				weights.Add( new KeyValuePair<int, int>(i, entry.m_weighting) );
				totalWeight += entry.m_weighting;
			}
		}

		int roll = Random.Range( 0, totalWeight );
		for( int i=0; i<weights.Count; ++i )
		{
			roll -= weights[i].Value;
			if( roll < 0 )
			{
				int index = weights[i].Key;
				return entries[index];
			}
		}
		return entries[0];
	}
}
