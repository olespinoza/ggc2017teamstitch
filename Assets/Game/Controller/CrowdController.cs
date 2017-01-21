using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController
{
	public static int UpdateCrowd( List<CrowdRow> rows, List<Wave> waves, float dt)
	{
		int result = 0;
		for (int i = 0; i < rows.Count; ++i)
		{
			CrowdRow row = rows [i];
			for (int j = 0; j < row.m_individuals.Count; ++j)
			{
				CrowdIndividual individual = row.m_individuals [j];
				if (individual != null) 
				{

					float prevAmplitude = WaveController.GetWaveStrengthAt (waves, individual.m_waveTheta - dt);
					float amplitude = WaveController.GetWaveStrengthAt (waves, individual.m_waveTheta);

					// fail
					if (prevAmplitude <= 0 && amplitude > 0 && individual.GetState () == CrowdIndividual.State.STATE_SITTING)
					{
						result++;
					}
					// start/continue wave
					else
					{
						individual.m_waveAmount = amplitude;
					}
				}
			}
		}
		return result;
	}

	public static List<CrowdRow> PopulateCrowd( string[] rowConfig, ProgressionManager prog )
	{
		List<CrowdRow> result = new List<CrowdRow>();

		for (int i = 0; i < 5 - rowConfig.Length; ++i)
		{
			result.Add (new CrowdRow ());
		}

		for(int i = 0; i < rowConfig.Length; ++i)
		{
			CrowdRow row = new CrowdRow();

			string[] seats = rowConfig[i].Split(',');

			for (int j = 0; j < seats.Length; ++j) 
			{
				string seatCfg = seats[j].Trim();
				if (seatCfg != "")
				{
					CrowdIndividual individual = new CrowdIndividual ();
					CfgCrowdConfig cccfg = prog.GetCrowdConfigById (seatCfg);
					individual.Setup (cccfg);
					row.m_individuals.Add (individual);
				} 
				else
				{
					row.m_individuals.Add (null);
				}
			}

			result.Add( row );
		}

		return result;
	}
}
