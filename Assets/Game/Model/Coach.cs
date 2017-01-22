using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coach
{
	public enum State
	{
		STANDING=0,
		CHEERING
	}

	public CfgCoach m_cfg;

	public float m_realPoint;
	public int m_slot;
	public int m_slots;
	public float m_cheer;

	public State GetState()
	{
		if (m_cheer > 0) { return State.CHEERING; }
		return State.STANDING;
	}
}
