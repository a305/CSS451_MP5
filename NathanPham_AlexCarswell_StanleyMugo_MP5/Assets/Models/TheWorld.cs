/// Created by Stanley Mugo on Nov 18, 2017
/// 
/// Manages selected objects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
	public int selectableLayer;
	private Selectable selectHandler = null;

	/// <summary>
	/// Deselect what ever is currently selected.
	/// </summary>
	public void ForceDeselect()
	{
		if (selectHandler != null)
		{
			selectHandler.OnForcedDeselect();
			selectHandler = null;
		}
	}

	/// <summary>
	/// Allows for the selected object to be set.
	/// Invokes the proper callback which a new value is selcted.
	/// </summary>
	/// <param name="mPos">The position that the mouse is pointing to.</param>
	/// <param name="g">The selected game object.</param>
	public void SetSelected(Vector3 mPos, ref GameObject g)
	{
		Selectable lastSelected = selectHandler;
		if (g != null)
		{
			Selectable iSelect = g.GetComponent<Selectable>();
			DeselectSelected(iSelect);

			if (iSelect != null)
			{
				if (iSelect.OnSelect(mPos, g, lastSelected))
					selectHandler = iSelect;
				else
					DeselectSelected();
			}
		} else
			DeselectSelected();

	}

	/// <summary>
	/// Allows for the selected object to be draged if it implements it.
	/// </summary>
	/// <param name="mPos">The position that the mouse is pointing to.</param>
	/// <param name="g">The selected game object.</param>
	public void DragedSelected(Vector3 mPos, ref GameObject g)
	{
		if (selectHandler != null)
			if (!selectHandler.OnDrag(mPos, g))
				DeselectSelected();
	}

	/// <summary>
	/// Checks if the selected object should be deselected when released.
	/// </summary>
	public void ReleaseSelected(Selectable toSelect = null)
	{
		if (selectHandler != null)
			if (!selectHandler.OnDeselect(toSelect))
				DeselectSelected();
	}

	/// <summary>
	/// Called when the user selects an empty region in space. Notifies
	/// the proper callback of the change in selection.
	/// </summary>
	public void DeselectSelected(Selectable toSelect = null)
	{
		if (selectHandler != null)
		{
			selectHandler.OnDeselect(toSelect);
			selectHandler = null;
		}
	}

	/// <summary>
	/// Returns whether their is shape that is currenly selected.
	/// </summary>
	/// <returns>True if there is a selected shape, else false.</returns>
	public bool HasSelected()
	{
		return selectHandler != null;
	}
}
