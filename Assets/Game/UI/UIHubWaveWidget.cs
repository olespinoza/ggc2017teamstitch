using UnityEngine;
using System.Collections;

public class UIHubWaveWidget : MonoBehaviour, IHub
{

	public void OnLevelChange( AppManager app )
	{

	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		if( gameState.m_waves.Count > 0 )
		{
			Wave wave = gameState.m_waves[0];
			float theta = (wave.Theta - 90.0f) * Mathf.Deg2Rad;
//			Vector2 cartesian = new Vector2( Mathf.Cos( theta ) * _spotRange.x, Mathf.Sin( theta ) * _spotRange.y );

			//_spot.anchoredPosition = cartesian;
		}
	}
}
