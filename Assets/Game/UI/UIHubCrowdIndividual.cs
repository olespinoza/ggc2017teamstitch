using UnityEngine;
using System.Collections;

public class UIHubCrowdIndividual : MonoBehaviour, IHub
{
	private UnityEngine.UI.Image _image;
	private int _row;
	private int _seat;

	private Sprite[] _sprites = null;

	public void Configure( int row, int seat )
	{
		_image = GetComponent<UnityEngine.UI.Image>();
		_row = row;
		_seat = seat;
	}

	public void OnLevelChange( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;
		CrowdIndividual model = gameState.m_rows[_row].m_individuals[_seat];

		_sprites = new Sprite[5];
		float w = model.m_config.m_asset.width;
		float h = model.m_config.m_asset.height;
		for (int i = 0; i < 5; ++i) 
		{
			Rect texRect = new Rect (i*w/5, 0, w/5, h);
			_sprites[i] = Sprite.Create (model.m_config.m_asset, texRect, Vector2.zero);
		}
		_image.sprite = _sprites[2];
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;
		CrowdIndividual model = gameState.m_rows[_row].m_individuals[_seat];

		UpdateImage( model );
	}

	public void UpdateImage( CrowdIndividual model )
	{
		if( _sprites == null ) return;

		switch( model.GetState() )
		{
		case CrowdIndividual.State.STATE_WAVING:
			int frameIdEst = (int)Mathf.Floor (model.m_waveAmount * 2);
			int clampedFrameId = Mathf.Clamp (frameIdEst, 0, 1);

			ChangeSprite (clampedFrameId + 3);
			break;
		case CrowdIndividual.State.STATE_GREEN:
			ChangeSprite (2);
			break;
		case CrowdIndividual.State.STATE_YELLOW:
			ChangeSprite (1);
			break;
		case CrowdIndividual.State.STATE_RED:
			ChangeSprite (0);
			break;
		}
	}

	public void ChangeSprite( int i )
	{
		if (_image.sprite != _sprites [i]) 
		{
			_image.sprite = _sprites [i];
		}
	}
}
