/// Created by Stanley Mugo Nov 19, 2017
/// 
/// Allows other objects to define what happens to this object on mouse events.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSelectable : MonoBehaviour, Selectable
{
	public delegate void ForcedDeselectHandler();
	public delegate bool DeselectHandler(Selectable newSelection);
	public delegate bool DragHandler(Vector3 mousePos, GameObject gObj);
	public delegate bool SelectHandler(Vector3 mousePos, GameObject gObj, Selectable lastSelection);

	public DeselectHandler onDeselect = null;
	public DragHandler onDrag = null;
	public SelectHandler onSelect = null;
	public ForcedDeselectHandler onForceDeselect = null;

	public virtual bool OnDeselect(Selectable newSelection)
	{
		if (onDeselect != null)
			return onDeselect.Invoke(newSelection);

		return false;
	}

	public virtual bool OnDrag(Vector3 mousePos, GameObject gObj)
	{
		if (onDrag != null)
			return onDrag.Invoke(mousePos, gObj);

		return false;
	}

	public virtual bool OnSelect(Vector3 mousePos, GameObject gObj, Selectable lastSelection)
	{
		if (onSelect != null)
			return onSelect.Invoke(mousePos, gObj, lastSelection);

		return false;
	}

	public virtual void OnForcedDeselect()
	{
		if (onForceDeselect != null)
			onForceDeselect();
	}
}
