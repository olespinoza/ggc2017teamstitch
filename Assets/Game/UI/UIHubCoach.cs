using UnityEngine;
using System.Collections;

public class UIHubCoach : MonoBehaviour, IHub
{
	[SerializeField] private float _xOrigin;

	public void OnLevelChange( AppManager app )
	{

	}
	public void UI( AppManager app )
	{
		Coach model = app.GameStateManager.GameState.m_coach;
		RectTransform rt = GetComponent<RectTransform>();

		float xRange = UIManager.STADIUM_WIDTH - 256;

		Vector3 position = rt.localPosition;			
		position.x = _xOrigin + xRange * ((float)model.m_slot / (float)(model.m_slots-1));
		rt.localPosition = position;
	}
}
