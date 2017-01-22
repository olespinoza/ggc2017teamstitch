using UnityEngine;
using System.Collections;

public class UIHubCoach : MonoBehaviour, IHub
{
	private UnityEngine.UI.Image _image;
	private Sprite[] _sprites;
	[SerializeField] private float _xOrigin;

	void Start()
	{
		_image = GetComponent<UnityEngine.UI.Image> ();
		_sprites = new Sprite[4];
		for (int i = 0; i < 2; ++i)
		{
			for (int j = 0; j < 2; ++j)
			{
				Rect rect = new Rect (j * UIManager.COACH_WIDTH, (1-i) * UIManager.COACH_HEIGHT, UIManager.COACH_WIDTH, UIManager.COACH_HEIGHT);
				_sprites [i * 2 + j] = Sprite.Create (_image.sprite.texture, rect, Vector2.zero);
			}
		}
	}

	public void OnLevelChange( AppManager app )
	{
		UpdateSprite (0);
	}

	public void UI( AppManager app )
	{
		Coach model = app.GameStateManager.GameState.m_coach;
		RectTransform rt = GetComponent<RectTransform>();

		float xRange = UIManager.STADIUM_WIDTH - 256;

		Vector3 position = rt.localPosition;			
		position.x = _xOrigin + xRange * ((float)model.m_slot / (float)(model.m_slots-1));
		rt.localPosition = position;

		if (model.m_cheer > 0) 
		{
			UpdateSprite (3);
		}
		else 
		{
			int[] frameMap = { 1, 0, 2, 0 };
			int frame = frameMap [Mathf.FloorToInt(Time.time * 2.0f) % 4];
			UpdateSprite (frame);
		}
	}

	public void UpdateSprite( int index )
	{
		if (_image.sprite != _sprites [index])
		{
			_image.sprite = _sprites [index];
		}
	}
}
