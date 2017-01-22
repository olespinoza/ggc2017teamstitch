using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
	private static readonly float AMPLITUDE_RATE = 6.0f;
	private CfgWaveEntry _cfg = null;

	private float _theta=0.0f;
	private float _omega=0.0f;
	private float _lastTheta=0.0f;
	private float _amplitude=0.0f;
	private float _width=0.0f;

	private bool _kill = false;
	private int _iterationsLeft = 0;

	public float Theta { get { return _cfg.m_invert ? _theta : 360.0f-_theta; } }
	public float LastTheta { get { return _cfg.m_invert ? _lastTheta : 360.0f-_lastTheta; } }

	public bool Invert { get { return _cfg.m_invert; } }
	public float Width { get { return _width; } }

	public Wave( CfgWaveEntry cfg )
	{
		_cfg = cfg;

		_theta = 0.0f;
		_lastTheta = 0.0f;
		_amplitude = 0.0f;
		_width = _cfg.m_width;

		_omega = 360.0f / _cfg.m_initialPeriod;
		_iterationsLeft = _cfg.m_iterations;
	}

	public float GetMagnitudeAt( float theta, bool prevFrame )
	{
		float distance = Mathf.Abs( theta - (prevFrame ? LastTheta : Theta) );
		return Mathf.Max( 0, 1.0f - distance / _width );
	}
		
	// Update is called once per frame
	public bool Update( float dt ) 
	{
		if (_kill)
		{
			_amplitude -= AMPLITUDE_RATE * dt;
			if (_amplitude < 0) {
				return true;
			}
		} 
		else
		{
			_amplitude = Mathf.Min (1.0f, _amplitude + AMPLITUDE_RATE * Time.deltaTime);
		}	

		_lastTheta = _theta;
		_theta += _omega * Time.deltaTime;
		if( _theta > 360.0f )
		{
			_theta -= 360.0f;
			_iterationsLeft--;
			if (_iterationsLeft <= 0) 
			{
				_kill = true;
			}
			else
			{
				float percent = (float)(_iterationsLeft-1) / (float)(_cfg.m_iterations-1);
				float period = _cfg.m_initialPeriod * (percent) + _cfg.m_finalPeriod * (1-percent);
				_omega = 360.0f / period;
			}
		}
		return false;
	}
}
