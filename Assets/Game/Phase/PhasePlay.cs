using UnityEngine;
using System.Collections;

public class PhasePlay : IPhase
{
	#region IPhase
	public void Setup( BaseAppManager app_ )
	{
		AppManager app = app_ as AppManager;

		app.GameStateManager.GameState.m_currentLevel = app.ProgressionManager.GetStartLevel () - 1;
		app.GameStateManager.NextLevel();
	}

	public void UpdateFixed( float dt )
	{
	}

	public bool UpdateFrame( float dt )
	{
		return false;
	}

	public void Teardown()
	{
	}
	#endregion
}
