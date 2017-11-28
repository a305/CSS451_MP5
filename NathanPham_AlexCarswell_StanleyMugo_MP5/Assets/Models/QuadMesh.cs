//#define DEBUG_ON



// Nathan, did I break something from your original code because the 'traigles.count' is
// is increasing indefinatly everytime 'ComputeNormals' is called.




// Nathan Pham
// CS451 Autumn 2017
// November 17th, 2017
// This script is used to calculate the initial meshing of the planar mesh
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMesh : AllMesh
{
    // Use this for initialization
    private void Start()
	{
		gameObject.AddComponent<MeshFilter>();
		size.x = 4;
		size.y = 4;
		SetResolution(4, 4);
	}

	public override void SetResolution(int numVertWidth, int numVertHeight)
	{
		mNormals = null;

		foreach (BindedPoint child in transform.GetComponentsInChildren<BindedPoint>())
			GameObject.Destroy(child.gameObject);
		
		Mesh theMesh = new Mesh();
		numRows = numVertWidth;
		numCol = numVertHeight;

		CreateNxMRectanlge(size.x, size.y, numRows - 1, numCol - 1, out v, out t);
		CalculateNormal(numRows - 1, numCol - 1, ref v, ref t, out n);

		theMesh.vertices = v; //  new Vector3[];
		theMesh.triangles = t; //  new int[];
		theMesh.normals = n;
		gameObject.GetComponent<MeshFilter>().mesh = theMesh;
		
		InitNormals(v, n);
	}

	void InitNormals(Vector3[] v, Vector3[] n)
	{
		GameObject prefabBoundPoint = Resources.Load("Prefabs\\MeshPoint") as GameObject;
		mNormals = new BindedPoint[v.Length];
		for (int i = 0; i < v.Length; i++)
		{
			GameObject o = GameObject.Instantiate(prefabBoundPoint);
			o.transform.SetParent(this.transform);
			o.name = "Normal" + i;
			o.AddComponent<Repositionable>();
			mNormals[i] = o.transform.GetComponentInChildren<BindedPoint>();
			mNormals[i].SetId(i);
			mNormals[i].MoveTo(v[i], v[i] + 1.0f * n[i]);
			mNormals[i].SetRadius(0.1f); // Radius of the sphere
			mNormals[i].SetNormalWidth(0.002f); // Width of the normal line
			mNormals[i].SetNormalLength(0.5f); // Height of the normal line
			mNormals[i].onMove = UpdateMesh;
			mNormals[i].gameObject.SetActive(false);
		}
	}
	
	void UpdateMesh(System.Object id, GameObject gObj)
	{
		Mesh theMesh = GetComponent<MeshFilter>().mesh;
		Vector3[] v = theMesh.vertices;
		Vector3[] n = theMesh.normals;

		v[(int)id] = gObj.transform.localPosition;
		
		CalculateNormal(numRows - 1, numCol - 1, ref v, ref t, out n);
		
		theMesh.vertices = v;
		theMesh.normals = n;

		// Update mesh point model
		for (int i = 0; i < mNormals.Length; i++)
			mNormals[i].PointTo(n[i]);
	}

	public static bool CreateNxMRectanlge(float n, float m, int widthResolution, int heightResolution, out Vector3[] verticies, out int[] triangles)
	{
		if (widthResolution < 0 || heightResolution < 0)
		{
			verticies = null;
			triangles = null;
			return false;
		}

		verticies = new Vector3[(widthResolution + 1) * (heightResolution + 1)];
		triangles = new int[widthResolution * heightResolution * 6];

		double squareWidth = (double)n / widthResolution;
		double squareHeight = (double)m / heightResolution;

		int curentTriangle = 0, triangleIndex = 0;
		for (int i = 0; i <= heightResolution; i++)
		{
			for (int k = 0; k <= widthResolution; k++)
			{
				verticies[curentTriangle] = new Vector3((float)(squareWidth * k - 0.5), 0,
												(float)(squareHeight * i - 0.5));


				if (i != heightResolution && k != widthResolution)
				{
					// First triangle
					triangles[triangleIndex++] = curentTriangle; // Top left point
					triangles[triangleIndex++] = curentTriangle + widthResolution + 1;// to Bottom left point
					triangles[triangleIndex++] = curentTriangle + widthResolution + 2;// to Bottom right point

					// Second triangle
					triangles[triangleIndex++] = curentTriangle; // Top left point
					triangles[triangleIndex++] = curentTriangle + widthResolution + 2;// to Bottom right point
					triangles[triangleIndex++] = curentTriangle + 1;// to Top right point
				}

				curentTriangle++;
			}
		}

		return true;
	}
}
