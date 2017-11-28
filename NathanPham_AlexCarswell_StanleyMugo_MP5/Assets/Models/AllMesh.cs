/// Created by Nathan Phan, Stanley Mugo
/// Base class for all manually created meshes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllMesh : MonoBehaviour
{
	protected BindedPoint[] mNormals;
	protected int numRows;
	protected int numCol;
	protected Vector3[] v;
	protected Vector3[] n;
	protected int[] t;
	protected bool showNormals = false;
	protected Vector2 size;

	public float widthRes
	{
		get { return numRows; }
	}

	public float heightRes
	{
		get { return numCol; }
	}

	public abstract void SetResolution(int numVertWidth, int numVertHeight);

	public void HideNormals()
	{
		for (int i = 0; i < transform.childCount; i++)
			if (transform.GetChild(i).GetComponent<BindedPoint>() != null)
				transform.GetChild(i).gameObject.SetActive(false);

		showNormals = false;
	}

	public void ShowNormals()
	{
		for (int i = 0; i < transform.childCount; i++)
			if (transform.GetChild(i).GetComponent<BindedPoint>() != null)
				transform.GetChild(i).gameObject.SetActive(true);

		showNormals = true;
	}

	public static Vector3 FaceNormal(Vector3[] v, int i0, int i1, int i2)
	{
		Vector3 a = v[i1] - v[i0];
		Vector3 b = v[i2] - v[i0];
		return Vector3.Cross(a, b).normalized;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="widthResolution">Number of squares making up the width.</param>
	/// <param name="heightResolution">Number of squares making up the height.</param>
	/// <param name="verticies"></param>
	/// <param name="triangles"></param>
	/// <param name="normals"></param>
	/// <returns>true if normals successfully calcuated, else false</returns>
	public static bool CalculateNormal(int widthResolution, int heightResolution, ref Vector3[] verticies, ref int[] triangles, out Vector3[] normals)
	{
		if (verticies == null || triangles == null || verticies.Length <= 0 || triangles.Length <= 0)
		{
			normals = null;
			return false;
		}

		normals = new Vector3[verticies.Length];

		int curentTriangle = 0;
		for (int i = 0; i <= heightResolution; i++)
		{
			for (int k = 0; k <= widthResolution; k++)
			{
				int tianglesInAverage = 1;
				if (i != 0)
				{
					if (k != 0)
					{
						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle - widthResolution - 1] - verticies[curentTriangle],
							verticies[curentTriangle - widthResolution - 2] - verticies[curentTriangle]).normalized;

						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle - widthResolution - 2] - verticies[curentTriangle],
							verticies[curentTriangle - 1] - verticies[curentTriangle]).normalized;

						tianglesInAverage += 2;
					}
					if (k != widthResolution)
					{
						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle + 1] - verticies[curentTriangle],
							verticies[curentTriangle - widthResolution - 1] - verticies[curentTriangle]).normalized;

						tianglesInAverage += 1;
					}
				}
				if (i != heightResolution)
				{
					if (k != 0)
					{
						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle - 1] - verticies[curentTriangle],
							verticies[curentTriangle + widthResolution + 1] - verticies[curentTriangle]).normalized;

						tianglesInAverage += 1;
					}
					if (k != widthResolution)
					{
						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle + widthResolution + 2] - verticies[curentTriangle],
							verticies[curentTriangle + 1] - verticies[curentTriangle]).normalized;

						normals[curentTriangle] += Vector3.Cross(verticies[curentTriangle + widthResolution + 1] - verticies[curentTriangle],
							verticies[curentTriangle + widthResolution + 2] - verticies[curentTriangle]).normalized;

						tianglesInAverage += 2;
					}
				}
				normals[curentTriangle] /= tianglesInAverage;
				curentTriangle++;
			}
		}

		return true;
	}
}
