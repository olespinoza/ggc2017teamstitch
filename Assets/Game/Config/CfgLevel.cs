using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CfgCrowdRow
{
	public string m_audience;
	public float m_stagger;
}

[System.Serializable]
public class CfgLevel
{
	public float m_thetaRange;
	public float m_incomingWarningRange;
	public int m_coachPositions;
	public CfgCrowdRow[] m_rows;
	public string m_coachId;

	public List<CfgWaveEntry> m_waveEntries = new List<CfgWaveEntry>();
}
