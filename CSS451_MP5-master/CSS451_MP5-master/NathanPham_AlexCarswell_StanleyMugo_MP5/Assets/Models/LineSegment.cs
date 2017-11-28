/// Created by Stayley Mugo Nov 18, 2017
/// 
/// Allows for simple intercations that allow a cylinder
/// gameobject to act like a line.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
	private Vector3 startPoint;
	private float _diameter;

	/// <summary>
	/// Diameter of the circular base of the cylindrical line.
	/// </summary>
	public float diameter
	{
		get { return _diameter; }
	}

	/// <summary>
	/// Distrance from the circular base bottom to the circular top base.
	/// </summary>
	public float height
	{
		get { return transform.localScale.y * 2; }
	}

	///<summary>
	///Sets the potition of the line using similar arguments as those found
	///in the line equation.
	///</summary>
	///<param name="start">The starting point of the line.</param>
	///<param name="dir">The direction that the line is going. Doesn't have to be normalized</param>
	///<param name="size">The length that the end point is away from the start point.</param>
	public void MoveTo(Vector3 start, Vector3 dir, float size)
	{
		startPoint = start;
		dir = dir.normalized;
		Vector3 transformUpNorm = transform.up.normalized;

		if (Mathf.Abs(Vector3.Dot(transformUpNorm, dir)) <= 0.99999f)
		{
			Quaternion rot = Quaternion.FromToRotation(transformUpNorm, dir);
			transform.localRotation = rot * transform.localRotation;
		}
		
		transform.localPosition = start + dir * size * 0.5f;
		transform.localScale = new Vector3(_diameter, size / 2, _diameter);
	}

	///<summary>
	///Sets the position of the line to a given starting point and ending point.
	///Because this line employes a  rotations must be done to get the
	///line to look correctly. Another moveTo function is called to avoid having
	///the same rotation code.
	///</summary>
	///<param name="start">The location that the line should start at.</param>
	///<param name="end">The location the lien should end at.</param>
	public void MoveTo(Vector3 start, Vector3 end)
	{
		Vector3 dir = end - start;
		MoveTo(start, dir, dir.magnitude);
	}

	/// <summary>
	/// Gets the normal vector pointing to the relative x axis of the line.
	/// </summary>
	/// <returns>Normalized direction of the line.</returns>
	public Vector3 Direction()
	{
		return transform.up.normalized;
	}

	/// <summary>
	/// Sets the width (diameter of circlular base) for the cylinder.
	/// </summary>
	/// <param name="w">The new width.</param>
	public void SetWidth(float w)
	{
		_diameter = w;
		transform.localScale = new Vector3(w, transform.localScale.y, w);
	}

	/// <summary>
	/// Sets the length of the line while maintaining the position and
	/// direction of the line.
	/// </summary>
	/// <param name="h">The new height.</param>
	public void SetLength(float h)
	{
		MoveTo(startPoint, transform.up, h);
	}

	/// <summary>
	/// Sets the directoin of the line while maintaining the starting position
	/// and the length of the line.
	/// </summary>
	/// <param name="d">A non-zero vector giving the direction.</param>
	public void SetDirection(Vector3 d)
	{
		MoveTo(startPoint, d, height);
	}
}