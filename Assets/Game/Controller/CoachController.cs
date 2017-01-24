﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachController
{
	public static void SetupCoach( bool firstLevel, Coach coach, CfgCoach cfg, int slots )
	{
		coach.m_cfg = cfg;
		int oldSlots = coach.m_slots;
		coach.m_slots = slots;
		coach.m_slot = Mathf.Max( coach.m_slot, slots -1 );
		coach.m_realPoint = firstLevel ? -5.0f : ( coach.m_realPoint * (float)slots / (float)oldSlots );
	}

	public static void UpdateCoach( UIManager ui, GameState gameState, float thetaRange, float finalLevelIndex, float dt )
	{
		Coach coach = gameState.m_coach;
		List<CrowdRow> rows = gameState.m_rows;

		if (gameState.m_levelFailedTicker > gameState.m_generalConfig.m_loseSavorDelay ||
		   (gameState.m_levelFinishedTicker > gameState.m_generalConfig.m_winSavorDelay && gameState.m_currentLevel >= finalLevelIndex)) {
			coach.m_slot = -3;
		} 
		else
		{
			if (Input.GetButtonDown ("left") && coach.m_slot > 0) 
			{
				coach.m_slot--;
			}

			if (Input.GetButtonDown ("right") && coach.m_slot < coach.m_slots - 1) {
				coach.m_slot++;
			}

			if (Input.GetButtonDown ("cheer") && coach.m_cheer <= 0) 
			{
				string[] cheerEffects = coach.m_cfg.m_cheerEffects;
				if (cheerEffects.Length > 0) {
					int effectId = Random.Range (0, cheerEffects.Length);
					ui.PlayEffect (cheerEffects [effectId]);
					GameObject coachBubble = GameObject.Find ("CoachBubble") as GameObject;
					coachBubble.GetComponent<AudioSource> ().Stop ();
					coachBubble.GetComponent<AudioSource> ().Play ();
				}
				coach.m_cheer = coach.m_cfg.m_cheerDuration;

				float theta = GetThetaFor (thetaRange, coach);

				CrowdController.PepUp (rows, theta, coach.m_cfg.m_cheerWidth);
			}
		}

		coach.m_cheer = Mathf.Max(0, coach.m_cheer - dt);

		if( !ui.IsSkippingNextFrame )
		{
			float moveStep = dt * coach.m_cfg.m_walkSpeed * coach.m_slots;
		
			if (Mathf.Abs (coach.m_realPoint - (float)coach.m_slot) < moveStep) 
			{
				coach.m_realPoint = coach.m_slot;
			}
			else if (coach.m_realPoint > (float)coach.m_slot) 
			{
				coach.m_realPoint -= moveStep;
			} 
			else if (coach.m_realPoint < (float)coach.m_slot) 
			{
				coach.m_realPoint += moveStep;
			}
			}

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit ();
		}
	}

	private static float GetThetaFor( float thetaRange, Coach coach )
	{
		float percent = (coach.m_realPoint / (float)(coach.m_slots - 1));
		return 180.0f - (percent - 0.5f) * thetaRange;
	}
}
