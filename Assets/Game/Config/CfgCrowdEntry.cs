using UnityEngine;
using System.Collections;

[System.Serializable]
public class CfgCrowdEntry
{
	public string m_crowdConfigId;

	public int m_weighting = 1;

	public int m_minRow=1;
	public int m_maxRow=4;
}
