using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CfgWaveEntry
{
	public float m_startTime = 0;
	public int m_iterations = 8;
	public float m_initialPeriod = 5.0f;
	public float m_periodMultiplierPerLoop = 0.9f;
	public float m_width = 5.0f;
}
