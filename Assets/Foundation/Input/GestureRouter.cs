using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IGestureSingleListener
{
	void OnSingleStart( GestureRouter.TouchData touch );
	void OnSingleFrame( GestureRouter.TouchData touch );
	void OnSingleMove( GestureRouter.TouchData touch, Vector2 delta );
	void OnSingleEnd( GestureRouter.TouchData touch );
}
public interface IGestureDualListener
{
	void OnDualStart( GestureRouter.TouchData touch1, GestureRouter.TouchData touch2 );
	void OnDualFrame( GestureRouter.TouchData touch1, GestureRouter.TouchData touch2 );
	void OnDualMove( GestureRouter.TouchData touch1, GestureRouter.TouchData touch2, Vector2 panDelta, float twistDelta, float zoomDelta );
	void OnDualEnd( GestureRouter.TouchData touch1, GestureRouter.TouchData touch2 );
}

public class GestureRouter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler
{
	public struct TouchData
	{
		public Vector2 positionOrigin;
		public Vector2 positionCurrent;
		public float startTime;
		public float duration;
		public bool moved;
	}

	private IGestureSingleListener _singleListener = null;
	private IGestureDualListener _dualListener = null;

	public float minimumDragThreshold;

	private TouchData _firstTouch;
	private TouchData _secondTouch;
	private int _recieveMode = 0;

	#region Monobehavior
	void Start()
	{
		_singleListener = (IGestureSingleListener)Util.GetComponentOfTypeOnGameObject<IGestureSingleListener>( gameObject );
		_dualListener = (IGestureDualListener)Util.GetComponentOfTypeOnGameObject<IGestureDualListener>( gameObject );
	}

	void Update()
	{
		if( _recieveMode == 1 )
		{
			_firstTouch.duration = Time.time - _firstTouch.startTime;
			if( _singleListener != null )
			{
				_singleListener.OnSingleFrame( _firstTouch );
			}
		}
		else if( _recieveMode == 2 )
		{
			_firstTouch.duration = Time.time - _firstTouch.startTime;
			_secondTouch.duration = Time.time - _secondTouch.startTime;
			if( _dualListener != null )
			{
				_dualListener.OnDualFrame( _firstTouch, _secondTouch );
			}
		}
	}
	#endregion

	#region IPointerDownHandler
	public void OnPointerDown( PointerEventData eventData )
	{
		// step from unused to single touch?
		if( (eventData.pointerId == -1 || eventData.pointerId == 0) && _recieveMode == 0 )
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
		// step from single touch to double touch?
		else if( eventData.pointerId == 1 && _recieveMode == 1 )
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
	#endregion

	#region IDragHandler
	public void OnDrag( PointerEventData eventData )
	{

		if( _recieveMode == 1 && (eventData.pointerId == -1 || eventData.pointerId == 0) )
		{
			if( _firstTouch.moved || (eventData.position - _firstTouch.positionOrigin).sqrMagnitude >= minimumDragThreshold*minimumDragThreshold )
			{
				_firstTouch.positionCurrent = eventData.position;
				_firstTouch.duration = Time.time - _firstTouch.startTime;
				_firstTouch.moved = true;

				if( _singleListener != null )
				{
					_singleListener.OnSingleMove( _firstTouch, eventData.delta );
				}
			}
		}
		if( _recieveMode == 2 )
		{
			Vector2 panDelta = eventData.delta / 2;
			if( eventData.pointerId == -1 || eventData.pointerId == 0 )
			{
				if( _firstTouch.moved || (eventData.position - _firstTouch.positionOrigin).sqrMagnitude >= minimumDragThreshold*minimumDragThreshold )
				{
					_firstTouch.positionCurrent = eventData.position;
					_firstTouch.duration = Time.time - _firstTouch.startTime;
					_firstTouch.moved = true;

					float spread = (_secondTouch.positionCurrent - _firstTouch.positionCurrent).magnitude;
					float oldSpread = (_secondTouch.positionCurrent - (_firstTouch.positionCurrent - eventData.delta)).magnitude;

					float twist = Mathf.Atan2( _secondTouch.positionCurrent.y - _firstTouch.positionCurrent.y, _secondTouch.positionCurrent.x - _firstTouch.positionCurrent.x );
					float oldTwist = Mathf.Atan2( _secondTouch.positionCurrent.y - (_firstTouch.positionCurrent.y - eventData.delta.y), _secondTouch.positionCurrent.x - (_firstTouch.positionCurrent.x - eventData.delta.x) );

					if( _dualListener != null )
					{
						_dualListener.OnDualMove( _firstTouch, _secondTouch, panDelta, twist-oldTwist, spread-oldSpread );
					}
				}
			}
			if( eventData.pointerId == 1 )
			{
				if( _secondTouch.moved || (eventData.position - _secondTouch.positionOrigin).sqrMagnitude >= minimumDragThreshold*minimumDragThreshold )
				{
					_secondTouch.positionCurrent = eventData.position;
					_secondTouch.duration = Time.time - _firstTouch.startTime;
					_secondTouch.moved = true;

					float spread = (_secondTouch.positionCurrent - _firstTouch.positionCurrent).magnitude;
					float oldSpread = ((_secondTouch.positionCurrent - eventData.delta) - _firstTouch.positionCurrent).magnitude;

					float twist = Mathf.Atan2( _secondTouch.positionCurrent.y - _firstTouch.positionCurrent.y, _secondTouch.positionCurrent.x - _firstTouch.positionCurrent.x );
					float oldTwist = Mathf.Atan2( (_secondTouch.positionCurrent.y - eventData.delta.y) - _firstTouch.positionCurrent.y, (_secondTouch.positionCurrent.x - eventData.delta.x) - _firstTouch.positionCurrent.x );

					if( _dualListener != null )
					{
						_dualListener.OnDualMove( _firstTouch, _secondTouch, panDelta, twist-oldTwist, spread-oldSpread );
					}
				}
			}
		}
	}
	#endregion

	#region IPointerUpHandler
	public void OnPointerUp( PointerEventData eventData )
	{

		if( _recieveMode == 1 && (eventData.pointerId == -1 || eventData.pointerId == 0) )
		{
			_recieveMode = 0;
			if( _singleListener != null )
			{
				_singleListener.OnSingleEnd( _firstTouch );
			}
		}
		else if( _recieveMode == 2 && (eventData.pointerId == -1 || eventData.pointerId == 0 || eventData.pointerId == 1) )
		{
			_recieveMode = 0;
			if( _dualListener != null )
			{
				_dualListener.OnDualEnd( _firstTouch, _secondTouch );
			}
		}
	}
	#endregion

	#region IScrollHandler
	// scroll delta forces zoom?
	public void OnScroll( PointerEventData eventData )
	{
		if( _dualListener != null )
		{
			_secondTouch.positionCurrent = _firstTouch.positionCurrent + Vector2.right * 15;
			_dualListener.OnDualMove( _firstTouch, _secondTouch, Vector2.zero, 0, eventData.scrollDelta.y );
		}
	}
	#endregion
}
