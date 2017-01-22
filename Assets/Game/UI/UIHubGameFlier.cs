using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHubGameFlier : MonoBehaviour, IHub
{
	public enum Stimulus
	{
		Win=0,
		Lose
	}

	private AudioSource _mainAudioSource;
	private UnityEngine.UI.Image _image;
	private float _fade = 0.0f;
	private bool _wasActive = false;
	[SerializeField] private Stimulus _stimulus;

	private void Start()
	{
		_image = GetComponent<UnityEngine.UI.Image> ();
		_mainAudioSource = Camera.main.GetComponent<AudioSource> ();
	}

	public void OnLevelChange( AppManager app )
	{
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;
		if (gameState.m_generalConfig != null) 
		{
			float fadeTime = gameState.m_generalConfig.m_uiTextFadeTime;

			bool active = false;
			switch (_stimulus) 
			{
			case Stimulus.Lose:
				active = gameState.m_levelFailedTicker > 0;
				break;
			case Stimulus.Win:
				active = gameState.m_levelFinishedTicker > 0;
				break;
			}

			if (active) 
			{
				// soundblah?
//			if (!_wasActive) 
//			{
//			
//			}
				_fade = Mathf.Clamp01 (_fade + Time.deltaTime * fadeTime);
				if (_fade != _image.color.a) 
				{
					_image.color = new Color (1, 1, 1, _fade);
				}
			}
			else
			{
				_fade = Mathf.Clamp01 (_fade - Time.deltaTime * fadeTime);
				if (_fade != _image.color.a) 
				{
					_image.color = new Color (1, 1, 1, _fade);
				}
			}

			_wasActive = active;
		}
	}
}
