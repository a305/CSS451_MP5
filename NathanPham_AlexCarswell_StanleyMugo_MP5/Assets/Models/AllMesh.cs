/// Created by Nathan Phan, Stanley Mugo
/// Base class for all manually created meshes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllMesh : MonoBehaviour
{
	public struct Triangle
	{
		int v1, v2, v3;
		public Triangle(int V1, int V2, int V3)
		{
			v1 = V1;
			v2 = V2;
			v3 = V3;
		}
		public int GetV1() { return v1; }
		public int GetV2() { return v2; }
		public int GetV3() { return v3; }
	}

	protected List<Triangle> triangles = new List<Triangle>();
	protected BindedPoint[] mNormals;
	protected int numVert = 4;
	protected int numRows;
	protected int numCol;
	protected Vector3[] v;
	protected Vector3[] n;
	protected int[] t;
	protected bool showNormals = false;

	public abstract void SetResolution(int numberOfVerticies);

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

	public static void ComputeNormals(int numCol, int numRows,
		ref Vector3[] v, ref Vector3[] n, ref List<Triangle> triangles)
	{
		Vector3[] triNormal = new Vector3[numCol * numRows * 2];
		int triInd = 0;

		// If using Visual Studio, extend the code using the + on the side
		#region Calculate_Normals_At_Each_Face
		// Perform the triangle counting cycle @ The initialize triangle mesh
		for (int i = 0; i < (numRows * numCol) - numRows - 1; i++)
		{
#if DEBUG_ON
			Debug.Log("N_Tri i: " + i);
#endif
			if ((i + 1) % (numRows) == 0) // if the next number is the edge, skip it
			{
#if DEBUG_ON
				Debug.Log("N_Skipped Tri i: " + i);
#endif
				i++;
#if DEBUG_ON
				Debug.Log("N_Now Tri i: " + i);
#endif
			}
			// first normal
			triNormal[triInd] = FaceNormal(v, i, i + numRows, i + numRows + 1);
			triangles.Add(new Triangle(i, i + numRows, i + numRows + 1));

			// second normal
#if DEBUG_ON
			Debug.Log("N_Triindex +1: " + (triInd + 1));
#endif
			triNormal[triInd + 1] = FaceNormal(v, i, i + numRows + 1, i + 1);
			triangles.Add(new Triangle(i, i + numRows + 1, i + 1));

			triInd += 2; // move to the next row
#if DEBUG_ON
			Debug.Log("N_Triindex : " + triInd);
#endif
		}
#endregion

#region Determine_Where_Triangles_Intersect
		for (int i = 0; i < n.Length; i++)
		{
			Vector3 norm = Vector3.zero;
			// perform superposition on any normals where the triangle connects each other
			for (int j = 0; j < triNormal.Length; j++)
			{
				Triangle tri = triangles[j];
				if (tri.GetV1() == i || tri.GetV2() == i || tri.GetV3() == i)
					norm += triNormal[j];
			}
			n[i] = norm.normalized; // the resulting vector is the average of the other normals
		}
#endregion
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
