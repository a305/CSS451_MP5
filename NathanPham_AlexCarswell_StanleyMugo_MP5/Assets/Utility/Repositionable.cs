/// Created by Stanley Mugo Nov 19, 2017
/// 
/// Makes the attatched object able to be moved like in the unity editor.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repositionable : CallbackSelectable
{
	public static PositionController posCtrl;
	private bool secondDeselect = false;

	/// <summary>
	/// Insure that this object is selected.
	/// </summary>
	public override bool OnDrag(Vector3 mousePos, GameObject gObj)
	{
		return true;
	}

	/// <summary>
	/// Allow being deselected by returning false.
	/// </summary>
	public override bool OnDeselect(Selectable newSelection)
	{
		if (newSelection == null && secondDeselect)
		{    // If clicked on empty space
			posCtrl.SetTarget(null); // Remove the position controller and Hide it
			return false;
		}

		base.OnDeselect(newSelection);
		secondDeselect = true;
		return true; // Keep as clicked so on next click, deselect will be called again.
	}

	/// <summary>
	/// On mouse first clicked, select self as what should be getting its position
	/// moved around.
	/// </summary>
	/// <returns>false to say that this object shouldn't be selected.</returns>
	public override bool OnSelect(Vector3 mousePos, GameObject gObj, Selectable lastSelected)
	{
		secondDeselect = false;
		posCtrl.SetTarget(gameObject); // Select self as what should be getting edited
		base.OnSelect(mousePos, gObj, lastSelected);
		return true; // Set as selected so OnMouseUp, deselect will be called
	}

	/// <summary>
	/// Hide the position controller and prevent it from having anything select to move.
	/// </summary>
	public override void OnForcedDeselect()
	{
		posCtrl.SetTarget(null);
		base.OnForcedDeselect();
	}
}
