using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
	public int m_currentLevel = -1;

	public float m_levelTime = 0;
	public float m_thetaRange = 0;

	public Coach m_coach = new Coach();

	public List<Wave> m_waves = new List<Wave>();
	public List<CrowdRow> m_rows = new List<CrowdRow>();
}
