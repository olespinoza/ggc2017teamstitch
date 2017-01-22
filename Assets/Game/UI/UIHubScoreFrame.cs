using UnityEngine;
using System.Collections;

public class UIHubScoreFrame : MonoBehaviour, IHub
{
	[SerializeField] private UnityEngine.UI.Text _score;
	[SerializeField] private UnityEngine.UI.Text _wave;
	[SerializeField] private UIProgressBar _progressBar;

	public void OnLevelChange( AppManager app )
	{

	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		_score.text = gameState.m_score.ToString("#,#");
		_wave.text = "Wave #" + gameState.m_wave.ToString();
		_progressBar.Value = (float)gameState.m_health / (float)gameState.m_maxHealth;
	}
}
