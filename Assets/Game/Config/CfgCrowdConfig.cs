using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CfgCrowdConfig
{
	public string m_id = "newCrowdConfig";

	public float m_yellowTime = 3.0f;
	public float m_greenTime = 8.0f;
	public float m_greenTimeExtra = 4.0f;

	public Texture2D m_asset;

	public int m_scorePerWave = 100;
}
