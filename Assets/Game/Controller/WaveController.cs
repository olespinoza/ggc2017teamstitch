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

	public static float CheckCrowdIndividual (List<Wave> waves, CrowdIndividual individual)
	{
		float prevAmplitude = 0;
		float amplitude = 0;
		for(int i = 0; i < waves.Count; ++i)
		{
			Wave wave = waves[i];

			float prevWaveMag = wave.GetMagnitudeAt(individual.m_theta, true);
			prevAmplitude = Mathf.Max(prevAmplitude, prevWaveMag);

			float waveMag = wave.GetMagnitudeAt(individual.m_theta, false);
			amplitude = Mathf.Max(amplitude, waveMag);

			// first impact
			if (prevWaveMag <= 0 && waveMag > 0) 
			{
				if (individual.GetState () == CrowdIndividual.State.STATE_RED) 
				{
					wave.AddRedHit ();
				}
				else if (individual.GetState () == CrowdIndividual.State.STATE_YELLOW)
				{
					wave.AddYellowHit ();
				}
			}
		}

		if ( prevAmplitude <= 0 && amplitude > 0 && individual.GetState () == CrowdIndividual.State.STATE_RED )
		{
			individual.m_failThisFrame = true;
		}
		else if (prevAmplitude <= 0 && amplitude > 0) 
		{
			individual.m_waveThisFrame = true;
		}

		return amplitude;
	}

}
