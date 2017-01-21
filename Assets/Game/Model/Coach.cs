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

	public int m_slot;
	public int m_slots;
	public float m_cheer;

	public State GetState()
	{
		if (m_cheer > 0) { return State.CHEERING; }
		return State.STANDING;
	}
}
