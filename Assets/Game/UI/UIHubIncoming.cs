using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHubIncoming : MonoBehaviour, IHub
{
	private UnityEngine.UI.Image _image;
	[SerializeField] private bool _invert;
	private AudioSource _audioSource;

	private void Start()
	{
		_image = GetComponent<UnityEngine.UI.Image> ();
		_audioSource = GetComponent<AudioSource> ();
	}

	public void OnLevelChange( AppManager app )
	{
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		if (gameState.m_currentLevel >= 0) 
		{
			CfgLevel level = app.ProgressionManager.GetLevelConfigByIndex (gameState.m_currentLevel);

			float blinkRate = 1.0f;
			bool on = false;
			for (int i = 0; i < gameState.m_waves.Count; ++i) 
			{
				Wave wave = gameState.m_waves [i];
				blinkRate = Mathf.Max (20.0f / wave.Period);
				if (wave.Invert && _invert) 
				{
					float theta = (wave.Theta) + wave.Width;
					float max = 180.0f - level.m_thetaRange / 2;
					float min = max - level.m_incomingWarningRange;
					if (theta > min && theta <= max) 
					{
						on = true;
					}
				}
				else if (!wave.Invert && !_invert) 
				{
					float theta = (360.0f - wave.Theta) + wave.Width;
					float max = 180.0f - level.m_thetaRange / 2;
					float min = max - level.m_incomingWarningRange;
					if (theta > min && theta <= max) 
					{
						on = true;
					}
				}
			}
			bool blink = (Mathf.FloorToInt (Time.time * blinkRate) % 2) == 0;
			_image.enabled = on && blink;

			if( _audioSource != null )
			{
				if( !_image.enabled && _audioSource.isPlaying == true ) _audioSource.Stop ();
				if( _image.enabled && _audioSource.isPlaying == false )_audioSource.Play ();
			}

		} 
		else
		{
			_image.enabled = false;
		}
	}
}
