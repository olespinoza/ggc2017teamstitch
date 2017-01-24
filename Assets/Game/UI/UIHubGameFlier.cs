﻿using UnityEngine;
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
	private AudioSource _overrideAudioSource;

	private Sprite _defaultSprite;

	private void Start()
	{
		_image = GetComponent<UnityEngine.UI.Image> ();
		_defaultSprite = _image.sprite;
		_mainAudioSource = Camera.main.GetComponent<AudioSource> ();
		_overrideAudioSource = GetComponent<AudioSource> ();
	}

	public void OnLevelChange( AppManager app )
	{
	}

	public void SwapTexture( Texture2D tex )
	{
		if (tex == null) 
		{
			_image.sprite = _defaultSprite;
		}
		else
		{
			_image.sprite = Sprite.Create(tex, new Rect (0, 0, tex.width, tex.height), Vector2.zero);
		}
		_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _image.sprite.textureRect.width);
		_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _image.sprite.textureRect.height);
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;
		if (gameState.m_generalConfig != null) 
		{
			float fadeTime = gameState.m_generalConfig.m_uiTextFadeTime;

			float transitionTicker = 0.0f;
			float transitionMax = 0.0f;
			bool active = false;
			bool soundCue = false;
			switch (_stimulus) 
			{
			case Stimulus.Lose:
				active = gameState.m_levelFailedTicker > 0;
				soundCue = active;
				transitionTicker = gameState.m_levelFailedTicker;
				transitionMax = gameState.m_generalConfig.m_loseSavorDelay;
				break;
			case Stimulus.Win:
				active = gameState.m_levelFinishedTicker > 0 || gameState.m_levelFadeInTicker > 0;
				if( gameState.m_levelFadeInTicker > 0 )
				{
					transitionTicker = gameState.m_generalConfig.m_levelScrollTime+gameState.m_levelFadeInTicker;
					transitionMax = gameState.m_generalConfig.m_levelScrollTime;
				}
				else
				{
					soundCue = active;
					transitionTicker = gameState.m_levelFinishedTicker;
					transitionMax = gameState.m_generalConfig.m_winSavorDelay;
				}
				break;
			}

			// vertical transition
			Vector3 lp = _image.rectTransform.localPosition;
			if( transitionTicker < gameState.m_generalConfig.m_levelScrollTime )
			{
				float percent = 1.0f - transitionTicker / gameState.m_generalConfig.m_levelScrollTime;
				lp.x = (percent*percent * 900);
			}
			else if( _stimulus == Stimulus.Win && transitionTicker > transitionMax )
			{				
				float percent = 1.0f - (transitionTicker-transitionMax) / transitionMax;
				lp.x = -(percent*percent * 900);
			}
			else if( transitionTicker > transitionMax - gameState.m_generalConfig.m_levelScrollTime )
			{
				float percent = (transitionTicker - (transitionMax - gameState.m_generalConfig.m_levelScrollTime)) / gameState.m_generalConfig.m_levelScrollTime;
				lp.x = -(percent*percent * 900);
			}
			else
			{
				lp.x = 0;
			}
			_image.rectTransform.localPosition = lp;

			if (soundCue && !_wasActive && _overrideAudioSource != null) 
			{
				_mainAudioSource.Stop ();
				_overrideAudioSource.Play ();
			}

			if (active) 
			{
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
