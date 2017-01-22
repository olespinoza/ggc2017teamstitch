using UnityEngine;
using System.Collections;

public class UIHubWaveWidget : MonoBehaviour, IHub
{
	[SerializeField] private UnityEngine.UI.Image _ccw;
	[SerializeField] private UnityEngine.UI.Image _cw;

	private float[] _widgetDensity = new float[256];

	public void OnLevelChange( AppManager app )
	{

	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		for (int i = 0; i < 256; ++i) 
		{
			_widgetDensity [i] = 0.0f;
		}

		bool showCCW = false;
		bool showCW = false;
		for (int i = 0; i < gameState.m_waves.Count; ++i) 
		{
			Wave wave = gameState.m_waves [i];
			if (wave.Invert)
			{
				showCCW = true;
			} 
			else
			{
				showCW = true;
			}

			float min = (wave.Theta - wave.Width)*256.0f/360.0f;
			float max = (wave.Theta + wave.Width)*256.0f/360.0f;
			for (int j = Mathf.FloorToInt (min); j <= Mathf.FloorToInt (max); ++j)
			{
				int k = j;
				while( k < 0 ) { k += 256; }
				while( k > 255 ) { k -= 256; }

				_widgetDensity [k] = 1.0f;
			}
		}

		_cw.enabled = showCW;
		_ccw.enabled = showCCW;
	}
}
