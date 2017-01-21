using UnityEngine;
using System.Collections.Generic;

public class GestureRouterFallthrough : MonoBehaviour
{
	private UnityEngine.EventSystems.EventSystem _eventSystem = null;

	private IGestureSingleListener _singleListener = null;
	private IGestureDualListener _dualListener = null;

	private GestureRouter.TouchData _firstTouch;
	private GestureRouter.TouchData _secondTouch;
	private int _recieveMode = 0;

	void Start()
	{
		_eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
	}

	void Update()
	{
		IExposedInputModule exposedModule = _eventSystem.currentInputModule as IExposedInputModule;
		if( exposedModule != null )
		{
			Dictionary<int, UnityEngine.EventSystems.PointerEventData> pointers = exposedModule.Pointers;
			if( pointers.ContainsKey(-1) && pointers[-1].selectedObject == null )
			{
				UnityEngine.EventSystems.PointerEventData eventData = pointers[-1];
				if( _recieveMode == 0 )
				{
					_recieveMode = 1;

					_firstTouch.positionOrigin = eventData.position;
					_firstTouch.positionCurrent = eventData.position;
					_firstTouch.startTime = Time.time;
					_firstTouch.duration = 0;
					_firstTouch.moved = false;

					if( _singleListener != null )
					{
						_singleListener.OnSingleStart( _firstTouch );
					}
				}
			}
			else
			{
				if( _recieveMode == 1 )
				{
					_recieveMode = 0;
					if( _singleListener != null )
					{
						_singleListener.OnSingleEnd( _firstTouch );
					}
				}
				else if( _recieveMode == 2 )
				{
					_recieveMode = 0;
					if( _dualListener != null )
					{
						_dualListener.OnDualEnd( _firstTouch, _secondTouch );
					}
				}
			}

			if( pointers.ContainsKey(0) && pointers[0].selectedObject == null )
			{
				UnityEngine.EventSystems.PointerEventData eventData = pointers[0];
				if( _recieveMode == 0 )
				{
					_recieveMode = 1;

					_firstTouch.positionOrigin = eventData.position;
					_firstTouch.positionCurrent = eventData.position;
					_firstTouch.startTime = Time.time;
					_firstTouch.duration = 0;
					_firstTouch.moved = false;

					if( _singleListener != null )
					{
						_singleListener.OnSingleStart( _firstTouch );
					}
				}
			}
			else
			{
				if( _recieveMode == 1 )
				{
					_recieveMode = 0;
					if( _singleListener != null )
					{
						_singleListener.OnSingleEnd( _firstTouch );
					}
				}
				else if( _recieveMode == 2 )
				{
					_recieveMode = 0;
					if( _dualListener != null )
					{
						_dualListener.OnDualEnd( _firstTouch, _secondTouch );
					}
				}
			}

			if( pointers.ContainsKey(1) && pointers[1].selectedObject == null )
			{
				UnityEngine.EventSystems.PointerEventData eventData = pointers[1];
				if( _recieveMode == 1 )
				{
					if( _singleListener != null )
					{
						_singleListener.OnSingleEnd( _firstTouch );
					}

					_recieveMode = 2;

					_secondTouch.positionOrigin = eventData.position;
					_secondTouch.positionCurrent = eventData.position;
					_secondTouch.startTime = Time.time;
					_secondTouch.duration = 0;
					_secondTouch.moved = false;

					if( _dualListener != null )
					{
						_dualListener.OnDualStart( _firstTouch, _secondTouch );
					}
				}
			}
			else
			{
				if( _recieveMode == 2 )
				{
					_recieveMode = 0;
					if( _dualListener != null )
					{
						_dualListener.OnDualEnd( _firstTouch, _secondTouch );
					}
				}
			}
		}
	}

	public void AttachListeners( IGestureSingleListener singleListener, IGestureDualListener dualListener )
	{
		_singleListener = singleListener;
		_dualListener = dualListener;
	}

	public void DetachListeners()
	{
		_singleListener = null;
		_dualListener = null;
	}
}
