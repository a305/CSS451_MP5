using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMesh : AllMesh
{
	private float rotation = Mathf.PI; // In radiuns

	// Use this for initialization
	void Start() {
		gameObject.AddComponent<MeshFilter>();
		size.x = 1;
		size.y = 2;
		SetResolution(10, 10);
	}

	public void SetCylinderRotation(float deg)
	{
		Mesh theMesh = GetComponent<MeshFilter>().mesh;
		rotation = deg * Mathf.Deg2Rad;
		int widthResolution = numRows - 1;
		int heightResolution = numCol - 1;
		
		int firsRowIndex = 0;
		for (int i = 0; i <= heightResolution; i++)
		{
			float r = (new Vector2(v[firsRowIndex].x, v[firsRowIndex].z)).magnitude; // Radius for this row

			for (int k = 1; k < numRows; k++)
			{
				v[firsRowIndex + k].x = r * Mathf.Cos(k * rotation / widthResolution);
				v[firsRowIndex + k].z = r * Mathf.Sin(k * rotation / widthResolution);
				mNormals[firsRowIndex + k].transform.localPosition = v[firsRowIndex + k];
				mNormals[firsRowIndex + k].transform.hasChanged = false;
			}

			firsRowIndex += numRows;
		}

		CalculateNormal(widthResolution, heightResolution, ref v, ref t, out n);
		theMesh.vertices = v;
		theMesh.normals = n;

		// Update mesh point model
		for (int i = 0; i < mNormals.Length; i++)
			mNormals[i].PointTo(n[i]);
	}

	public override void SetResolution(int numVertWidth, int numVertHeight)
	{
		foreach (BindedPoint child in transform.GetComponentsInChildren<BindedPoint>())
			GameObject.Destroy(child.gameObject);

		Mesh theMesh = new Mesh();
		numRows = numVertWidth;
		numCol = numVertHeight;

		// Instatiate Verticies & triangles
		CreateNxMVerticies(size.x, size.y, out v, out t);
		//ComputeNormals(numRows, numCol, ref v, ref n, ref triangles);

		CalculateNormal(numRows - 1, numCol - 1, ref v, ref t, out n);

		theMesh.vertices = v; //  new Vector3[];
		theMesh.triangles = t; //  new int[];
		theMesh.normals = n;
		
		if (gameObject.GetComponent<MeshFilter>() != null)
			gameObject.GetComponent<MeshFilter>().mesh = theMesh;

		InitNormals(ref v, ref n);
	}

	void InitNormals(ref Vector3[] v, ref Vector3[] n)
	{
		GameObject prefabBoundPoint = Resources.Load("Prefabs\\MeshPoint") as GameObject;
		mNormals = new BindedPoint[v.Length];
		for (int i = 0; i < v.Length; i++)
		{
			GameObject o = GameObject.Instantiate(prefabBoundPoint);
			o.transform.SetParent(this.transform);
			o.name = "Normal" + i;

			if (!showNormals)
				o.SetActive(false);

			mNormals[i] = o.transform.GetComponentInChildren<BindedPoint>();
			mNormals[i].SetRadius(0.1f); // Radius of the sphere
			mNormals[i].SetNormalWidth(0.002f); // Width of the normal line
			mNormals[i].SetNormalLength(0.5f); // Height of the normal line
			mNormals[i].SetId(i);
			mNormals[i].MoveTo(v[i], v[i] + 1.0f * n[i]);
			mNormals[i].transform.hasChanged = false;
			mNormals[i].onMove = UpdateMesh;

			if (i % numRows == 0)
			{
				o.AddComponent<Repositionable>();
			}
			else
			{
				mNormals[i].gameObject.layer = 0;
				mNormals[i].GetComponent<MeshRenderer>().material.color = Color.black;
			}
		}
	}

	private void UpdateMesh(System.Object id, GameObject gObj)
	{
		Mesh theMesh = GetComponent<MeshFilter>().mesh;		
		v[(int)id] = gObj.transform.localPosition;

		int widthResolution = numRows - 1;

		int firsRowIndex = (int)id - ((int)id % numRows);
		float r = (new Vector2(v[(int)id].x, v[(int)id].z)).magnitude; // Radius for this row
		for (int i = 1; i < numRows; i++)
		{
			v[firsRowIndex + i].y = v[(int)id].y;
			v[firsRowIndex + i].x = r * Mathf.Cos(i * rotation / widthResolution);
			v[firsRowIndex + i].z = r * Mathf.Sin(i * rotation / widthResolution);
			mNormals[firsRowIndex + i].transform.localPosition = v[firsRowIndex + i];
			mNormals[firsRowIndex + i].transform.hasChanged = false;
		}

		CalculateNormal(numRows - 1, numCol - 1, ref v, ref t, out n);
		theMesh.vertices = v;
		theMesh.normals = n;
	}

	private void CreateNxMVerticies(float n, float m, out Vector3[] verticies, out int[] triangles)
	{
		int widthResolution = numRows - 1;
		int heightResolution = numCol - 1;

		verticies = new Vector3[(widthResolution + 1) * (heightResolution + 1)];
		triangles = new int[widthResolution * heightResolution * 6];

		int curentTriangle = 0, triangleIndex = 0;
		for (int i = 0; i <= heightResolution; i++)
		{
			for (int k = 0; k <= widthResolution; k++)
			{
				verticies[curentTriangle] = new Vector3(0, m * i / (float) heightResolution, 0);
				verticies[curentTriangle].x = n * 0.5f * Mathf.Cos(k * rotation / widthResolution);
				verticies[curentTriangle].z = n * 0.5f * Mathf.Sin(k * rotation / widthResolution);

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
	}
}
