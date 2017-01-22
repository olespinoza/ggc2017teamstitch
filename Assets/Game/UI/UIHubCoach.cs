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
	}

	public void OnLevelChange( AppManager app )
	{
		Coach model = app.GameStateManager.GameState.m_coach;
		Texture2D tex = model.m_cfg.m_asset;
			
		float w = _image.sprite.texture.width;
		float h = _image.sprite.texture.height;
		_sprites = new Sprite[4];
		for (int i = 0; i < 2; ++i)
		{
			for (int j = 0; j < 2; ++j)
			{
				Rect rect = new Rect (j * w/2, (1-i) * h/2, w/2, h/2);
				_sprites [i * 2 + j] = Sprite.Create (tex, rect, Vector2.zero);
			}
		}

		UpdateSprite (0);
	}

	public void UI( AppManager app )
	{
		Coach model = app.GameStateManager.GameState.m_coach;
		if (model.m_cfg != null) 
		{
			RectTransform rt = GetComponent<RectTransform> ();

			float xRange = UIManager.STADIUM_WIDTH - 256;

			Vector3 position = rt.localPosition;			
			position.x = _xOrigin + xRange * (model.m_realPoint / (float)(model.m_slots - 1));
			rt.localPosition = position;

			if (model.m_cheer > 0)
			{
				UpdateSprite (3);
			} 
			else if (Mathf.Abs (model.m_realPoint - (float)model.m_slot) < 0.01f && model.m_cfg.m_animateWhileStill == false) 
			{
				UpdateSprite (0);
			} 
			else 
			{
				int[] frameMap = { 1, 0, 2, 0 };
				int frame = frameMap [Mathf.FloorToInt (Time.time * model.m_cfg.m_animationFrameRate) % 4];
				UpdateSprite (frame);
			}
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
