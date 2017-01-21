using UnityEngine;
using System.Collections;

public class ManagerInitData
{
}

public interface IManager
{
	void Setup( ManagerInitData initData );
	void Teardown();
	void UpdateFrame( float dt );
	void UpdateFrameLate( float dt );
	void UpdateFixed( float dt );
}
