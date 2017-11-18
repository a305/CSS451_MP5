// Nathan Pham
// CS451 Autumn 2017
// November 17th, 2017
// This script is used to calculate the initial meshing of the planar mesh
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMesh : MonoBehaviour
{
    public int numVert = 4;
    private int numRows;
    private int numCol;
    private Vector3[] v;
    private Vector3[] n;
    private int[] t;
    // Use this for initialization
    void Start () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there
        numRows = numVert;
        numCol = numVert;
        // Intuition: think of one triangle: 3 vertices,
        // the product in parentheses is the area. Half that amount
        int numtriangleVert = 3 * ( (numRows - 1 + numCol - 1) * (numRows - 1 + numCol - 1) )/ 2; 
        Debug.Log("Number of triangle verts: " + numtriangleVert);

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

        InitControllers(v);
        InitNormals(v, n);
    }

    // Creates the vertices by stating: for each step in the column direction,
    // step all the way to the end of the row direction, assign a vertex to that point.
    void InitializeVertices()
    {
        float dRow = transform.localScale.z / (numRows); // change in the row direction
        float dCol = transform.localScale.x / (numCol); // change in the column direction
        int index = 0;
        Debug.Log("dRow : " + dRow);
        Debug.Log("dCol : " + dCol);
        Debug.Log("numCol : " + numCol);
        Debug.Log("numRow : " + numRows);
        Debug.Log("# verts " +v.Length);

        for (int i = 0; i < numCol; i++) // for each point in x
        {
            for (int j = 0; j < numRows; j++) // for each point in z
            {
                v[index] = new Vector3(i * dCol, 0, -j * dRow); // assign the vertex at that index
                Debug.Log("Vector V [" + (index) + "] : " + v[index].ToString());
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
        Debug.Log("Mesh stop condition: " + ((numRows * numCol) - numRows - 1));
                            // for example, 3x3: stop at 5
        for(int i = 0; i < (numRows * numCol) - numRows - 1; i++)
        {
            Debug.Log("Tri i: " + i);
            if ((i + 1) % (numRows) == 0) // if the next number is the edge, skip it
            {
                Debug.Log("Skipped Tri i: " + i);
                i++;
                Debug.Log("Now Tri i: " + i);
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
            Debug.Log("Triindex : " + triIndex);
        }
        int ind = 0; // Debug code // 
        foreach (int i in t)
        {
            Debug.Log("triangle vert: " + ind + " : " + i);
            ind++;
        }
    }

    // Update is called once per frame
    void Update () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);

        theMesh.vertices = v;
        theMesh.normals = n;
	}
}
