using UnityEngine;
using System.Collections;

public interface IPhase
{
	void Setup( BaseAppManager app );
	void Teardown();
	void UpdateFixed( float dt );
	bool UpdateFrame( float dt );
}
