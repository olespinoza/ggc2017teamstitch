using UnityEngine;
using System.Collections;

public class UIHubScoreFrame : MonoBehaviour, IHub
{
	[SerializeField] private UnityEngine.UI.Text _score;
	[SerializeField] private UnityEngine.UI.Text _wave;
	[SerializeField] private UnityEngine.UI.Text _health;

	[SerializeField] private UIProgressBar _progressBar;

	public void OnLevelChange( AppManager app )
	{

	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;

		_score.text = gameState.m_score.ToString("#,#");
		_wave.text = "Wave #" + gameState.m_waveIndex.ToString() + "/" + gameState.m_waveCount.ToString();
		_progressBar.Value = (float)gameState.m_health / (float)gameState.m_maxHealth;
		_health.text = gameState.m_health.ToString () + "/" + gameState.m_maxHealth.ToString ();
	}
}
