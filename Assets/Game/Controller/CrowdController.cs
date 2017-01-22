using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController
{
	public static KeyValuePair<int , int> UpdateCrowd( GameState gameState, float dt)
	{
		List<CrowdRow> rows = gameState.m_rows;
		List<Wave> waves = gameState.m_waves;

		int score = 0;
		int damage = 0;
		for (int i = 0; i < rows.Count; ++i)
		{
			CrowdRow row = rows [i];
			for (int j = 0; j < row.m_individuals.Count; ++j)
			{
				CrowdIndividual individual = row.m_individuals [j];
				if (individual != null) 
				{
					individual.m_waveThisFrame = false;
					individual.m_failThisFrame = false;
					float amplitude = WaveController.CheckCrowdIndividual (waves, individual);

					if( individual.m_failThisFrame )
					{
						damage++;
					}
					if (individual.m_waveThisFrame) 
					{
						score += individual.m_config.m_scorePerWave;
					}

					// start/continue wave
					else if( individual.GetState() != CrowdIndividual.State.STATE_RED )
					{
						individual.m_waveAmount = amplitude;
					}

					if (gameState.m_levelFinishedTicker > 0) 
					{
						individual.ResetEnergy ();
					}
					else 
					{
						individual.m_energy -= dt;
					}
				}
			}
		}
		return new KeyValuePair<int, int>(score,damage);
	}

	public static void PepUp( List<CrowdRow> rows, float theta, float width )
	{
		for (int i = 0; i < rows.Count; ++i)
		{
			CrowdRow row = rows [i];
			for (int j = 0; j < row.m_individuals.Count; ++j)
			{
				CrowdIndividual individual = row.m_individuals [j];
				if (individual != null) 
				{
					if (Mathf.Abs (individual.m_theta - theta) < width) 
					{
						individual.ResetEnergy ();
					}
				}
			}
		}
	}

	public static List<CrowdRow> PopulateCrowd( CfgCrowdRow[] rowConfig, ProgressionManager prog )
	{
		List<CrowdRow> result = new List<CrowdRow>();

		for (int i = 0; i < 5 - rowConfig.Length; ++i)
		{
			result.Add (new CrowdRow ());
		}

		for(int i = 0; i < rowConfig.Length; ++i)
		{
			CrowdRow row = new CrowdRow();
			row.m_stagger = rowConfig[i].m_stagger;

			string[] seats = rowConfig[i].m_audience.Split(',');

			for (int j = 0; j < seats.Length; ++j) 
			{
				string seatCfg = seats[j].Trim();
				if(seatCfg != "")
				{
					CrowdIndividual individual = new CrowdIndividual();
					CfgCrowdConfig cccfg = prog.GetCrowdConfigById(seatCfg);
					individual.Setup(cccfg);
					row.m_individuals.Add(individual);
				}
				else
				{
					row.m_individuals.Add(null);
				}
			}

			result.Add( row );
		}

		return result;
	}
}
