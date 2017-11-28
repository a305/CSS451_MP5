/// Created by Stayley Mugo Nov 18, 2017
/// 
/// Used to set reciever to update when this game object
/// is moved arround.
/// Remarks: The object attached to this script is always assumed to be
/// spherical so that the width = height = length.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindedPoint : MonoBehaviour
{
	public delegate void OnMoved(System.Object data, GameObject pModel);
	private System.Object identifyingInformation;
	public OnMoved onMove;

	/// <summary>
	/// Sets the radius of this sphere.
	/// </summary>
	/// <param name="r">Half of the diameter</param>
	public void SetRadius(float r)
	{
		transform.localScale = Vector3.one * r;
		transform.hasChanged = false;
	}
	
	/// <summary>
	/// Sets the width of the normal line. This is the width of the circular base
	/// of the cylinder that makes up the normal line.
	/// </summary>
	/// <param name="w">Any number</param>
	public void SetNormalWidth(float w)
	{
		// The following assumes that this sphere is always circular
		transform.GetComponentInChildren<LineSegment>()
			.SetWidth(w / transform.localScale.y);
	}

	/// <summary>
	/// Sets the height of the normal line.
	/// </summary>
	/// <param name="h"></param>
	public void SetNormalLength(float h)
	{
		// The following assumes that this sphere is always circular
		transform.GetComponentInChildren<LineSegment>()
			.SetLength((h + transform.localScale.y / 4) / transform.localScale.y);
	}

	/// <summary>
	/// Set some value to return with with the call back to id this pertiruar
	/// point from the others in the mesh.
	/// </summary>
	/// <param name="objId">Any object</param>
	public void SetId(System.Object objId)
	{
		identifyingInformation = objId;
	}

	/// <summary>
	/// Moves the sphere so that the normal line starts at the supllied
	/// <code>start</code> position and ends at the supplied <code>end</code>
	/// position.
	/// </summary>
	/// <param name="start">Point in world space</param>
	/// <param name="end">Point in world space</param>
	public void MoveTo(Vector3 start, Vector3 end)
	{
		transform.localPosition = start;
		LineSegment child = transform.GetComponentInChildren<LineSegment>();
		child.MoveTo(Vector3.zero, end - start, child.height);

		// Place the bottom in the middle of the sphere
		//child.transform.localPosition = child.transform.up * child.transform.localScale.y;
	}

	/// <summary>
	/// Points the line segment at the given direction while keeping on end of the
	/// line in the same position. The end that doesn't move was that supplied
	/// as the start point in the call to <code>MoveTo</code>.
	/// </summary>
	/// <param name="dir">Non-zero vector, normalized or not</param>
	public void PointTo(Vector3 dir)
	{
		transform.GetComponentInChildren<LineSegment>().SetDirection(dir);
	}

	/// <summary>
	/// Calls a listener if transform is updated
	/// </summary>
	private void Update()
	{
		if (transform.hasChanged)
		{
			if (onMove != null)
				onMove.Invoke(identifyingInformation, gameObject);

			transform.hasChanged = false;
		}
	}
}
