using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdIndividual
{
	public enum State
	{
		STATE_STANDING=0,
		STATE_FLAGGING,
		STATE_SITTING,
		STATE_WAVING,
	}

	public CfgCrowdConfig m_config;
	public float m_energy;

	public float m_waveAmount;
	public float m_waveTheta; // -- Set by UI, in case we want some interesting tailoring

	public void Setup( CfgCrowdConfig config )
	{
		m_config = config;
		m_waveAmount = 0;

		ResetEnergy();
	}

	public void ResetEnergy()
	{
		m_energy = m_config.m_boredomDuration + m_config.m_baseBoredomDelay + Random.Range( 0, m_config.m_additionalBoredomRange );
	}

	public State GetState()
	{
		if( m_waveAmount > 0 ) { return State.STATE_WAVING; }

		if( m_energy < 0 ) { return State.STATE_SITTING; }

		if( m_energy < m_config.m_boredomDuration ) { return State.STATE_FLAGGING; }

		return State.STATE_STANDING;
	}
}

public class CrowdRow
{
	public List<CrowdIndividual> m_individuals = new List<CrowdIndividual>();
}
