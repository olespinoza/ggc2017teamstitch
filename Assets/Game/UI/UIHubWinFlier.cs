using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHubWinFlier : MonoBehaviour, IHub
{
	private UnityEngine.UI.Image _image;

	private void Start()
	{
		_image = GetComponent<UnityEngine.UI.Image> ();
	}

	public void OnLevelChange( AppManager app )
	{
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		_image.enabled = gameState.m_levelFinishedTicker > 0;
	}
}
