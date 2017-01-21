using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController 
{
	public static List<Wave> GetNewWavesForTimeRange( CfgLevel lcfg, float oldTime, float newTime)
	{
		List<Wave> waves = new List<Wave>();

		for( int i=0; i<lcfg.m_waveEntries.Count; ++i )
		{
			CfgWaveEntry waveCfg = lcfg.m_waveEntries[i];
			if( oldTime < waveCfg.m_startTime && newTime >= waveCfg.m_startTime )
			{
				waves.Add( new Wave( waveCfg ) );
			}
		}

		return waves;
	}

	public static bool UpdateWaves( List<Wave> waves, float dt )
	{
		for( int i = 0; i < waves.Count; ++i )
		{
			Wave wave = waves [i];
			if( wave.Update(dt) )
			{
				waves.RemoveAt(i);
				--i;
			}
		}
		return waves.Count == 0;
	}

	public static float GetWaveStrengthAt( List<Wave> waves, float theta )
	{
		float amplitude = 0;
		for(int i = 0; i < waves.Count; ++i)
		{
			Wave wave = waves[i];
			float waveMag = wave.GetMagnitudeAt(theta);
			amplitude = Mathf.Max(amplitude, waveMag);
		}
		return amplitude;
	}
}
