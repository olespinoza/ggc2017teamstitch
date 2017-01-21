using UnityEngine;
using System.Collections.Generic;

public class ExposedStandaloneInputModule : UnityEngine.EventSystems.StandaloneInputModule, IExposedInputModule
{
	public Dictionary<int, UnityEngine.EventSystems.PointerEventData> Pointers { get { return m_PointerData; } }
}
