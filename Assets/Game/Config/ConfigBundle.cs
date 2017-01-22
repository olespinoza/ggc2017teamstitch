using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "cfg000", menuName = "Bundles/ConfigBundle", order = 1)]
[System.Serializable]
public class ConfigBundle : ScriptableObject
{
	public int m_startLevel = 0;
	public List< CfgLevel > m_levels = new List< CfgLevel >();
	public List< CfgCrowdConfig > m_crowdConfigs = new List< CfgCrowdConfig >();
	public List< CfgCoach > m_coaches = new List< CfgCoach >();
}
