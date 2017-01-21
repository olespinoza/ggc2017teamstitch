using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CfgLevel
{
	public float m_thetaRange;
	public int m_coachPositions;
	public string[] m_rows;

	public List<CfgWaveEntry> m_waveEntries = new List<CfgWaveEntry>();
}
