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
		SetResolution(4);
	}

	public override void SetResolution(int numberOfVerticies)
	{
		triangles.Clear();
		mNormals = null;

		foreach (BindedPoint child in transform.GetComponentsInChildren<BindedPoint>())
			GameObject.Destroy(child.gameObject);

		if (numberOfVerticies >= 2)
			numVert = numberOfVerticies;

		Mesh theMesh = new Mesh();
		numRows = numCol = numVert;

		// Intuition: think of one triangle: 3 vertices,
		// the product in parentheses is the area. Half that amount
		int numtriangleVert = 3 * ((numRows - 1 + numCol - 1) * (numRows - 1 + numCol - 1)) / 2;
#if DEBUG_ON
        Debug.Log("Number of triangle verts: " + numtriangleVert);
#endif

		v = new Vector3[numRows * numCol];   // The number of vertices we need is
											 // the number of rows times the number of columns
		t = new int[numtriangleVert]; // number of vertices used to make each triangle
		n = new Vector3[numRows * numCol];   // MUST be the same as number of vertices

		InitializeVertices();
		UpdateMeshTriangles();
		for (int i = 0; i < n.Length; i++) // set all normals
			n[i] = new Vector3(0, 1, 0);

		theMesh.vertices = v; //  new Vector3[];
		theMesh.triangles = t; //  new int[];
		theMesh.normals = n;
		gameObject.GetComponent<MeshFilter>().mesh = theMesh;

		//InitControllers(v);        // -------------------------
		InitNormals(v, n);
	}

    // Creates the vertices by stating: for each step in the column direction,
    // step all the way to the end of the row direction, assign a vertex to that point.
    void InitializeVertices()
    {
        float dRow = transform.localScale.z / (numRows); // change in the row direction
        float dCol = transform.localScale.x / (numCol); // change in the column direction
        int index = 0;

#if DEBUG_ON
		Debug.Log("dRow : " + dRow);
        Debug.Log("dCol : " + dCol);
        Debug.Log("numCol : " + numCol);
        Debug.Log("numRow : " + numRows);
        Debug.Log("# verts " +v.Length);
#endif
		for (int i = 0; i < numCol; i++) // for each point in x
        {
            for (int j = 0; j < numRows; j++) // for each point in z
            {
                v[index] = new Vector3(i * dCol, 0, -j * dRow); // assign the vertex at that index
#if DEBUG_ON
                Debug.Log("Vector V [" + (index) + "] : " + v[index].ToString());
#endif
                index++;
            }
        }
    }

    // does the counting algorithm of the triangle
    // Follows: At the upper left corner, go counterclockwise to mark 
    // the triangular vertices (ex, 0 (up left), 1 (up right), 2(low left), 3(low right)
    //                          1st tri: 0, 2, 3: 2nd tri: 0, 3, 1)
    void UpdateMeshTriangles()
    {
        int triIndex = 0;
#if DEBUG_ON
        Debug.Log("Mesh stop condition: " + ((numRows * numCol) - numRows - 1));
#endif
		// for example, 3x3: stop at 5
		for (int i = 0; i < (numRows * numCol) - numRows - 1; i++)
        {
#if DEBUG_ON
            Debug.Log("Tri i: " + i);
#endif
			if ((i + 1) % (numRows) == 0) // if the next number is the edge, skip it
            {
#if DEBUG_ON
                Debug.Log("Skipped Tri i: " + i);
#endif
				i++;
#if DEBUG_ON
                Debug.Log("Now Tri i: " + i);
#endif
			}
			// first triangle
			t[triIndex] = i;
            t[triIndex + 1] = i + numRows;
            t[triIndex + 2] = i + numRows + 1;

            // second triangle
            t[triIndex + 3] = i;
            t[triIndex + 4] = i + numRows + 1;
            t[triIndex + 5] = i + 1;
            triIndex += 6; // move to the next row
#if DEBUG_ON
            Debug.Log("Triindex : " + triIndex);
#endif
		}
		int ind = 0; // Debug code // 
        foreach (int i in t)
        {
#if DEBUG_ON
            Debug.Log("triangle vert: " + ind + " : " + i);
#endif
			ind++;
        }
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
			mNormals[i].SetRadius(0.1f); // Radius of the sphere
			mNormals[i].SetNormalWidth(0.002f); // Width of the normal line
			mNormals[i].SetNormalHeight(0.5f); // Height of the normal line
			mNormals[i].SetId(i);
			mNormals[i].onMove = UpdateMesh;
		}
		UpdateNormals(v, n);
	}

	void UpdateNormals(Vector3[] v, Vector3[] n)
	{
		for (int i = 0; i < v.Length; i++)
		{
			mNormals[i].MoveTo(v[i], v[i] + 1.0f * n[i]);
			mNormals[i].transform.hasChanged = false;
		}
	}
	
	void UpdateMesh(System.Object id, GameObject gObj)
	{
		Mesh theMesh = GetComponent<MeshFilter>().mesh;
		Vector3[] v = theMesh.vertices;
		Vector3[] n = theMesh.normals;

		v[(int)id] = gObj.transform.localPosition;

		//AllMesh.ComputeNormals(numCol, numRows, ref v, ref n, ref triangles); // Broken
		CalculateNormal(numCol - 1, numRows - 1, ref v, ref t, out n);
		
		theMesh.vertices = v;
		theMesh.normals = n;
	}
}
