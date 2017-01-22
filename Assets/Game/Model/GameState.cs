using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
	public CfgGeneral m_generalConfig; 

	public int m_currentLevel = -1;

	public float m_levelTime = 0;
	public float m_thetaRange = 0;

	public float m_levelFailedTicker = 0;
	public float m_levelFinishedTicker = 0;

	public int m_health = 0;
	public int m_maxHealth = 0;

	public int m_score = 0;
	public int m_waveIndex = 0;
	public int m_waveCount = 0;

	public Coach m_coach = new Coach();

	public List<Wave> m_waves = new List<Wave>();
	public List<CrowdRow> m_rows = new List<CrowdRow>();

	public void ResetLevel()
	{
		m_maxHealth = m_generalConfig.m_maxHealth;
		m_levelTime = 0;
		m_levelFinishedTicker = 0;
		m_levelFailedTicker = 0;
		m_health = m_maxHealth;
	}
}
