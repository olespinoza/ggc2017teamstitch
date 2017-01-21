using UnityEngine;
using System.Collections.Generic;

public interface IExposedInputModule
{
	Dictionary<int, UnityEngine.EventSystems.PointerEventData> Pointers { get; }
}
