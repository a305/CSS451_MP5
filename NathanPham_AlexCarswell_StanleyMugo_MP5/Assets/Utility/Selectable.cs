using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Selectable
{
	bool OnSelect(Vector3 mousePos, GameObject gObj, Selectable lastSelected);
	bool OnDeselect(Selectable newSelection);
	bool OnDrag(Vector3 mousePos, GameObject gObj);
	void OnForcedDeselect();
}