using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachController
{
	static readonly float CHEER_DURATION = 1.0f;

	public static void UpdateCoach( UIManager ui, Coach coach, List<CrowdRow> rows, float dt )
	{
		if(Input.GetButtonDown("left") && coach.m_slot > 0 )
		{
			coach.m_slot--;
		}

		if(Input.GetButtonDown("right") && coach.m_slot < coach.m_slots-1 )
		{
			coach.m_slot++;
		}

		if (Input.GetButtonDown ("cheer"))
		{
			ui.PlayEffect ("CoachBubble");
			coach.m_cheer = CHEER_DURATION;
		}

		coach.m_cheer = Mathf.Max(0, coach.m_cheer - dt);
	}
}
