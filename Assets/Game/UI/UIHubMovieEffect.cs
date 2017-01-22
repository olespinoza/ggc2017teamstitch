using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHubMovieEffect : MonoBehaviour, IHub
{
	private UnityEngine.UI.RawImage _movie;

	public void OnLevelChange( AppManager app )
	{
		_movie = GetComponent<UnityEngine.UI.RawImage>();
	}

	public void UI( AppManager app )
	{
	}

	public void Play()
	{
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
	}
}
