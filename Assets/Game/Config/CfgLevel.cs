using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CfgRowEntry
{
	public int m_seats;
}

[System.Serializable]
public class CfgLevel
{
	public float m_thetaRange;
	public CfgRowEntry[] m_rows;

	public List<CfgCrowdEntry> m_crowdEntries = new List<CfgCrowdEntry>();
	public List<CfgWaveEntry> m_waveEntries = new List<CfgWaveEntry>();
}
