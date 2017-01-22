using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414

public class UIHubMovieEffect : MonoBehaviour, IHub
{
	private UnityEngine.UI.RawImage _movie;
	[SerializeField] private UnityEngine.UI.Image _fallbackImage;
	[SerializeField] private float _fallbackImageDuration;
	private float _fallbackTicker=0;

	public void OnLevelChange( AppManager app )
	{
		#if UNITY_WEBGL

		#else
		_movie = GetComponent<UnityEngine.UI.RawImage>();
		#endif
	}

	public void UI( AppManager app )
	{
		#if UNITY_WEBGL
		if( _fallbackImage != null )
		{
			_fallbackTicker -= Time.deltaTime;
			if( _fallbackTicker <= 0 )
			{
				_fallbackImage.enabled = false;
			}
		}
		#else
		if (_movie != null) 
		{
			Texture main = _movie.material.mainTexture;
			MovieTexture mainMovie = main as MovieTexture;
			_movie.enabled = mainMovie.isPlaying;
		}
		#endif
	}

	public void Play()
	{
		#if UNITY_WEBGL
		if (_fallbackImage != null)
		{
			_fallbackTicker = _fallbackImageDuration;
			_fallbackImage.enabled = true;
		}
		#else
		if (_movie != null)
		{
			Texture mask = _movie.material.GetTexture("_Mask");
			Texture main = _movie.material.mainTexture;
			MovieTexture maskMovie = mask as MovieTexture;
			MovieTexture mainMovie = main as MovieTexture;
			maskMovie.Stop ();
			maskMovie.Play ();

			mainMovie.Stop ();
			mainMovie.Play ();
		}
		#endif
	}
}
