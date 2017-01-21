using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CfgCrowdConfigAnimation
{
	public float m_framerate = 8.0f;
	public Texture2D[] m_images;
}

[System.Serializable]
public class CfgCrowdConfig
{
	public string m_id = "newCrowdConfig";

	public float m_boredomDuration = 3.0f;
	public float m_baseBoredomDelay = 8.0f;
	public float m_additionalBoredomRange = 4.0f;

	public CfgCrowdConfigAnimation m_sitting;
	public CfgCrowdConfigAnimation m_flagging;
	public CfgCrowdConfigAnimation m_standing;
	public CfgCrowdConfigAnimation m_waving;
}
