using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHubCrowdRow : MonoBehaviour, IHub
{
	[SerializeField] private UIHubCrowdIndividual _crowdIndividualPrefab;
	[SerializeField] private int _rowId;

	private List<UIHubCrowdIndividual> _individuals = new List<UIHubCrowdIndividual>();
	private List<UIHubCrowdIndividual> _oldCrowd = new List<UIHubCrowdIndividual>();

	void Start()
	{
	}

	public void OnLevelChange( AppManager app )
	{
		_oldCrowd.AddRange( _individuals );
		_individuals.Clear();

		GameState gameState = app.GameStateManager.GameState;
		if(gameState.m_rows.Count > _rowId)
		{
			CrowdRow rowState = gameState.m_rows [_rowId];

			float spacing = (UIManager.STADIUM_WIDTH - UIManager.CHARACTER_WIDTH) / (gameState.m_coach.m_slots-1);
			float stagger = rowState.m_stagger;

			float leftTheta = 180.0f - gameState.m_thetaRange / 2;
			float rightTheta = 180.0f + gameState.m_thetaRange / 2;
			for(int i = 0; i < rowState.m_individuals.Count; ++i)
			{
				if (rowState.m_individuals[i] != null)
				{
					GameObject go = GameObject.Instantiate (_crowdIndividualPrefab.gameObject);
					go.name = "Individual " + (i + 1).ToString ();
					go.transform.SetParent (transform);
					float x = (stagger + i) * spacing;
					go.transform.localPosition = new Vector3(x, 0, 0);
					go.transform.localScale = Vector3.one;
		
					float percent = x / (UIManager.STADIUM_WIDTH-UIManager.CHARACTER_WIDTH); // not sure how to derive this; fuck it.
					gameState.m_rows [_rowId].m_individuals[i].m_theta = percent * leftTheta + (1 - percent) * rightTheta;

					UIHubCrowdIndividual uiHCI = go.GetComponent<UIHubCrowdIndividual> ();
					uiHCI.Configure (_rowId, i);
					uiHCI.OnLevelChange (app);
					uiHCI.x_offset = (gameState.m_currentLevel>0) ? 960.0f : 0.0f;
					_individuals.Add (uiHCI);
				}
			}
		}
	}

	public void UI( AppManager app )
	{
		for( int i=0; i<_oldCrowd.Count; ++i )
		{
			_oldCrowd[i].x_offset -= (960.0f * Time.deltaTime / app.GameStateManager.GameState.m_generalConfig.m_levelScrollTime);
			if( _oldCrowd[i].x_offset <= -960.0f )
			{
				Destroy( _oldCrowd[i].gameObject );
				_oldCrowd.RemoveAt(i);
				--i;
			}
			else
			{
				_oldCrowd[i].DeadUI( app );
			}
		}

		for( int i=0; i<_individuals.Count; ++i )
		{
			if( _individuals[i].x_offset > 0 )
			{
				_individuals[i].x_offset -= (960.0f * Time.deltaTime / app.GameStateManager.GameState.m_generalConfig.m_levelScrollTime);
				_individuals[i].x_offset = Mathf.Max( 0.0f, _individuals[i].x_offset );
			}

			_individuals[i].UI( app );
		}
	}
}
