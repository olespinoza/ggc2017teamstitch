using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CfgCoach
{
	public string m_id;

	public float m_cheerDuration = 1.0f;
	public float m_cheerWidth = 10.0f;
	public string[] m_cheerEffects;
	public float m_walkSpeed = 1.0f;

	public float m_animationFrameRate = 2.0f;
	public bool m_animateWhileStill = false;

	public Texture2D m_asset;
}