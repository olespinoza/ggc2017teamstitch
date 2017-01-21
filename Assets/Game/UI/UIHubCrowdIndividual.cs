using UnityEngine;
using System.Collections;

public class UIHubCrowdIndividual : MonoBehaviour, IHub
{
	private UnityEngine.UI.Image _image;
	private int _row;
	private int _seat;

	private Texture2D _currentTex = null;

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

		//UpdateImage( model );
	}

	public void UI( AppManager app )
	{
		GameState gameState = app.GameStateManager.GameState;
		CrowdIndividual model = gameState.m_rows[_row].m_individuals[_seat];

		UpdateImage( model );
	}

	public void UpdateImage( CrowdIndividual model )
	{
		if( model.m_waveAmount > 0 )
		{
			CfgCrowdConfigAnimation anim = model.m_config.m_waving;
			int frameIdEst = (int)Mathf.Floor( model.m_waveAmount * anim.m_images.Length );
			int clampedFrameId = Mathf.Clamp( frameIdEst, 0, anim.m_images.Length-1 );
			AssignTexture( anim.m_images[clampedFrameId] );

		}
		else
		{
			CfgCrowdConfigAnimation anim = null;
			switch( model.GetState() )
			{
			case CrowdIndividual.State.STATE_SITTING:
				anim = model.m_config.m_sitting;
				break;
			case CrowdIndividual.State.STATE_FLAGGING:
				anim = model.m_config.m_flagging;
				break;
			case CrowdIndividual.State.STATE_STANDING:
				anim = model.m_config.m_standing;
				break;
			}

			AssignTexture( CalcFrameTexture( model.m_energy, anim ) );
		}
	}

	public Texture2D CalcFrameTexture( float energy, CfgCrowdConfigAnimation anim )
	{
		float frameTime = anim.m_framerate * energy;
		int frameId = (int)Mathf.Abs(Mathf.Floor(frameTime)) % anim.m_images.Length;
		return anim.m_images[frameId];
	}

	public void AssignTexture( Texture2D tex )
	{
		if( tex != _currentTex )
		{
			int frameId = 0;
			Rect texRect = new Rect(UIManager.CHARACTER_WIDTH*frameId, 0, UIManager.CHARACTER_WIDTH, tex.height);
			_image.sprite = Sprite.Create( tex, texRect, Vector2.zero );
			_currentTex = tex;
		}
	}
}
